using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Data;
using Ayudantia.Src.Dtos;
using Ayudantia.Src.Helpers;
using Ayudantia.Src.Mappers;
using Ayudantia.Src.Models;

using Microsoft.AspNetCore.Mvc;

namespace Ayudantia.Src.Controllers
{
    public class BasketController(ILogger<ProductController> logger, UnitOfWork unitOfWork) : BaseController
    {
        private readonly ILogger<ProductController> _logger = logger;
        private readonly UnitOfWork _unitOfWork = unitOfWork;
        [HttpGet]
        public async Task<ActionResult<ApiResponse<BasketDto>>> GetBasket()
        {
            var basket = await RetrieveBasket();
            if (basket == null)
                return NoContent();

            return Ok(new ApiResponse<BasketDto>(
                true,
                "Carrito obtenido correctamente",
                basket.ToDto()
            ));
        }

        [HttpPost]
        public async Task<ActionResult<ApiResponse<BasketDto>>> AddItemToBasket(int productId, int quantity)
        {
            _logger.LogWarning("Entrando a AddItemToBasket con productId: {ProductId}, quantity: {Quantity}", productId, quantity);

            var basket = await RetrieveBasket();

            if (basket == null)
            {
                basket = CreateBasket();
                await _unitOfWork.SaveChangeAsync();
            }

            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);
            if (product == null)
                return BadRequest(new ApiResponse<string>(false, "Producto no encontrado"));

            if (product.Stock == 0)
                return BadRequest(new ApiResponse<string>(false, $"El producto '{product.Name}' no tiene stock disponible."));

            if (product.Stock < quantity)
                return BadRequest(new ApiResponse<string>(false, $"Solo hay {product.Stock} unidades disponibles de '{product.Name}'"));

            basket.AddItem(product, quantity);

            var changes = await _unitOfWork.SaveChangeAsync();
            var success = changes > 0;

            return success
                ? CreatedAtAction(nameof(GetBasket), new ApiResponse<BasketDto>(true, "Producto añadido al carrito", basket.ToDto()))
                : BadRequest(new ApiResponse<string>(false, "Ocurrió un problema al actualizar el carrito"));
        }


        [HttpDelete]
        public async Task<ActionResult<ApiResponse<BasketDto>>> RemoveItemFromBasket(int productId, int quantity)
        {
            var basket = await RetrieveBasket();
            if (basket == null)
                return BadRequest(new ApiResponse<string>(false, "Carrito no encontrado"));

            basket.RemoveItem(productId, quantity);

            var success = await _unitOfWork.SaveChangeAsync() > 0;

            return success
                ? Ok(new ApiResponse<BasketDto>(
                    true,
                    "Producto eliminado del carrito",
                    basket.ToDto()
                ))
                : BadRequest(new ApiResponse<string>(false, "Error al actualizar el carrito"));
        }

        private async Task<Basket?> RetrieveBasket()
        {
            var basketId = Request.Cookies["basketId"];
            _logger.LogWarning("BasketId recibido desde cookie: {BasketId}", basketId);

            return string.IsNullOrEmpty(basketId)
                ? null
                : await _unitOfWork.BasketRepository.GetBasketAsync(basketId);
        }

        private Basket CreateBasket()
        {
            var basketId = Guid.NewGuid().ToString();
            var cookieOptions = new CookieOptions
            {
                IsEssential = true,
                Expires = DateTime.UtcNow.AddDays(30),
            };

            Response.Cookies.Append("basketId", basketId, cookieOptions);
            _logger.LogWarning("Nuevo basket creado con ID: {BasketId}", basketId);

            return _unitOfWork.BasketRepository.CreateBasket(basketId);
        }
    }
}