using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayudantia.Src.Dtos
{
    public class BasketDto
    {
        public required string BasketId { get; set; }

        public List<BasketItemDto> Items { get; set; } = [];

        public double TotalPrice { get; set; }
    }
}