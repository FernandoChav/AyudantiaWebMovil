using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayudantia.Src.Models
{
    public class OrderItem
    {
        public int Id { get; set; }

        public int Quantity { get; set; }
        public decimal Price { get; set; }  // Precio congelado al momento de la compra
        public string ProductName { get; set; } = string.Empty;

        // Clave externa del producto (opcional para trazabilidad)
        public int ProductId { get; set; }

        // Relación con Order
        public int OrderId { get; set; }
        public Order Order { get; set; } = null!;
    }
}