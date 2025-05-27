using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Dtos;
using Ayudantia.Src.Models;

namespace Ayudantia.Src.Mappers
{
    public static class BasketMapper
    {
        public static BasketDto ToDto(this Basket basket)
        {
            return new BasketDto
            {
                BasketId = basket.BasketId,
                Items = basket.Items.Select(i => new BasketItemDto
                {
                    ProductId = i.ProductId,
                    Name = i.Product.Name,
                    Price = i.Product.Price,
                    PictureUrl = i.Product.Urls != null && i.Product.Urls.Any() ? i.Product.Urls.First() : null,
                    Quantity = i.Quantity,
                    Brand = i.Product.Brand,
                    Category = i.Product.Category
                }).ToList(),
                TotalPrice = (double)basket.Items.Sum(i => i.Quantity * i.Product.Price) // ✅ TOTAL
            };
        }
    }
}