using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace SisNikosPizza.Domain.Models
{
    public class Carrito
    {
        public int CarritoId { get; set; }
        public string OwnerId { get; set; }

        // Relación con el usuario
        public IdentityUser Owner { get; set; }

        // Relación con los productos a través de CarritoProducto
        public List<CarritoProducto>? CarritoProductos { get; set; } = new List<CarritoProducto>();
    }
}
