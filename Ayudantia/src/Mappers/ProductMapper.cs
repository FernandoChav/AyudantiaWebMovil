using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Ayudantia.Src.Dtos;
using Ayudantia.Src.Models;

namespace Ayudantia.Src.Mappers
{
    public static class ProductMapper
    {
        public static Product FromCreateDto(ProductDto dto, List<string> urls, string? publicId = null)
        {
            return new Product
            {
                Name = dto.Name,
                Description = dto.Description,
                Price = dto.Price,
                Stock = dto.Stock,
                Brand = dto.Brand,
                Category = dto.Category,
                Urls = urls,
                PublicId = publicId
            };
        }
    }
}