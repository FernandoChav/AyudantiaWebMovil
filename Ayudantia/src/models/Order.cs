using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ayudantia.Src.Models
{
    public class Order
    {
        public int Id { get; set; }

        // Usuario que hizo el pedido
        public string UserId { get; set; } = string.Empty;
        public User User { get; set; } = null!;

        // Dirección (copiada del usuario al momento de la orden)
        public string Address { get; set; } = string.Empty;

        // Fecha y estado
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;
        public string Status { get; set; } = "Creado"; // Podría usarse enum

        // Total pagado
        public decimal Total { get; set; }

        // Productos comprados
        public List<OrderItem> Items { get; set; } = new();
    }
}