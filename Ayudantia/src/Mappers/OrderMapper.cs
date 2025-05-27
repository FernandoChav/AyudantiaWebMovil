using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Dtos;
using Ayudantia.Src.Models;

namespace Ayudantia.Src.Mappers
{
    public static class OrderMapper
    {
        public static Order FromBasket(Basket basket, string userId, int shippingAddressId)
        {
            return new Order
            {
                UserId = userId,
                ShippingAddressId = shippingAddressId,
                Total = basket.Items.Sum(i => i.Quantity * i.Product.Price),
                Items = basket.Items.Select(i => new OrderItem
                {
                    ProductId = i.Product.Id,
                    ProductName = i.Product.Name,
                    Quantity = i.Quantity,
                    Price = i.Product.Price
                }).ToList()
            };
        }

        public static OrderDto ToOrderDto(Order order, Dictionary<int, Product> productCache)
        {
            return new OrderDto
            {
                Id = order.Id,
                CreatedAt = order.OrderDate,
                Address = ShippingAddressMapper.ToDto(order.ShippingAddress),
                Total = (int)Math.Floor(order.Total),
                Items = order.Items.Select(item =>
                {
                    var product = productCache.GetValueOrDefault(item.ProductId);
                    return new OrderItemDto
                    {
                        ProductId = item.ProductId,
                        Name = item.ProductName,
                        Quantity = item.Quantity,
                        Price = (int)Math.Floor(item.Price),
                        ImageUrl = product?.Urls?.FirstOrDefault() ?? ""
                    };
                }).ToList()
            };
        }

        public static OrderSummaryDto ToSummaryDto(Order order)
        {
            return new OrderSummaryDto
            {
                Id = order.Id,
                CreatedAt = order.OrderDate,
                Total = (int)Math.Floor(order.Total)
            };
        }
    }
}