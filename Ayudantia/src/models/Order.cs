using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayudantia.Src.Models
{
    public class Order
    {
        public int Id { get; set; }

        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;

        // NUEVO: FK a ShippingAddress
        public int ShippingAddressId { get; set; }
        public ShippingAddres ShippingAddress { get; set; } = null!;

        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Creado";

        public decimal Total { get; set; }

        public List<OrderItem> Items { get; set; } = new();
    }
}