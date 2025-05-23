using Microsoft.AspNetCore.Identity;

namespace SisNikosPizza.Domain.Models
{
    public class CarritoProducto
    {
        public Producto Producto { get; set; }
        public int UserId { get; set; }
        public int Cantidad { get; set; }
    }
}
