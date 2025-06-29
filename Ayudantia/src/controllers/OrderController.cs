using System.Security.Claims;

using Ayudantia.Src.Data;
using Ayudantia.Src.Dtos;
using Ayudantia.Src.Helpers;
using Ayudantia.Src.Mappers;


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Ayudantia.Src.Controllers;

public class OrderController(ILogger<OrderController> logger, UnitOfWork unitOfWork) : BaseController
{
    private readonly UnitOfWork _unitOfWork = unitOfWork;
    private readonly ILogger<OrderController> _logger = logger;

    [HttpPost]
    public async Task<ActionResult<ApiResponse<OrderDto>>> CreateOrder()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

        var addres = await _unitOfWork.ShippingAddressRepository.GetByUserIdAsync(userId);
        if (addres == null || addres.Number == "" || addres.Street == "" || addres.Commune == "" || addres.Region == "" || addres.PostalCode == "")
            return BadRequest(new ApiResponse<string>(false, "No tienes una dirección completa registrada. Por favor agrégala antes de comprar."));

        var basketId = Request.Cookies["basketId"];
        if (string.IsNullOrEmpty(basketId))
            return BadRequest(new ApiResponse<string>(false, "No se encontró el carrito"));

        var basket = await _unitOfWork.BasketRepository.GetBasketAsync(basketId);
        if (basket == null || !basket.Items.Any())
            return BadRequest(new ApiResponse<string>(false, "El carrito está vacío"));

        var order = OrderMapper.FromBasket(basket, userId, addres.Id);

        // Validar y reducir stock
        foreach (var item in order.Items)
        {
            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(item.ProductId);
            if (product == null)
                return BadRequest(new ApiResponse<string>(false, $"Producto con ID {item.ProductId} no encontrado."));

            product.Stock -= item.Quantity;

            if (product.Stock < 0)
                return BadRequest(new ApiResponse<string>(false, $"No hay suficiente stock para el producto {product.Name}"));
            if (product.Stock == 0)
                product.IsActive = false;
        }

        await _unitOfWork.OrderRepository.CreateOrderAsync(order);
        _unitOfWork.BasketRepository.DeleteBasket(basket);
        await _unitOfWork.SaveChangeAsync();

        // 🔁 Cargar productos para el mapeo completo con imagenes
        var productIds = order.Items.Select(i => i.ProductId).ToList();
        var products = await _unitOfWork.ProductRepository.GetProductsByIdsAsync(productIds);
        var productDict = products.ToDictionary(p => p.Id);

        var dto = OrderMapper.ToOrderDto(order, productDict);

        return Ok(new ApiResponse<OrderDto>(true, "Pedido realizado correctamente", dto));
    }


    [Authorize(Roles = "User")]
    [HttpGet]
    public async Task<ActionResult<ApiResponse<IEnumerable<OrderSummaryDto>>>> GetMyOrders()
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

        var orders = await _unitOfWork.OrderRepository.GetOrdersByUserIdAsync(userId);
        var mapped = orders.Select(OrderMapper.ToSummaryDto).ToList();

        return Ok(new ApiResponse<IEnumerable<OrderSummaryDto>>(true, "Historial de pedidos obtenido", mapped));
    }


    [HttpGet("{id}")]
    public async Task<ActionResult<ApiResponse<OrderDto>>> GetOrderById(int id)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId is null)
            return Unauthorized(new ApiResponse<string>(false, "Usuario no autenticado"));

        var order = await _unitOfWork.OrderRepository.GetOrderByIdAsync(id, userId);
        if (order == null)
            return NotFound(new ApiResponse<OrderDto>(false, "Pedido no encontrado"));

        var productIds = order.Items.Select(i => i.ProductId).ToList();
        var products = await _unitOfWork.ProductRepository.GetProductsByIdsAsync(productIds);
        var productDict = products.ToDictionary(p => p.Id);

        var dto = OrderMapper.ToOrderDto(order, productDict);

        return Ok(new ApiResponse<OrderDto>(true, "Pedido encontrado", dto));
    }
}