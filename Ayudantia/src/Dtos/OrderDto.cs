using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayudantia.Src.Dtos
{
    public class OrderDto
    {

        public int Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Address { get; set; } = string.Empty;
        public int Total { get; set; } // En CLP, sin decimales
        public List<OrderItemDto> Items { get; set; } = [];
    }
}