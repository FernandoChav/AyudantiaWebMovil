using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Data;
using Ayudantia.Src.Dtos;
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
        public async Task<ActionResult<BasketDto>> GetBasket()
        {
            var basket = await RetrieveBasket();
            if (basket == null) return NoContent();
            return basket.ToDto();
        }

        [HttpPost]
        public async Task<ActionResult<BasketDto>> AddItemToBasket(int productId, int quantity)
        {
            _logger.LogWarning("Entrando a AddItemToBasket con productId: {ProductId}, quantity: {Quantity}", productId, quantity);
            var basket = await RetrieveBasket();

            if (basket == null)
            {
                basket = CreateBasket();
                await _unitOfWork.SaveChangeAsync();
            }

            var product = await _unitOfWork.ProductRepository.GetProductByIdAsync(productId);
            if (product == null) return BadRequest("Product not found");

            basket.AddItem(product, quantity);

            var changes = await _unitOfWork.SaveChangeAsync();
            var success = changes > 0;
            return success ? CreatedAtAction(nameof(GetBasket), basket.ToDto()) : BadRequest("Problem updating basket");
        }
        [HttpDelete]
        public async Task<ActionResult> RemoveItemFromBasket(int productId, int quantity)
        {
            var basket = await RetrieveBasket();
            if (basket == null) return BadRequest("Basket not found");

            basket.RemoveItem(productId, quantity);

            var success = await _unitOfWork.SaveChangeAsync() > 0;
            return success ? Ok(basket.ToDto()) : BadRequest("Problem updating basket");
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