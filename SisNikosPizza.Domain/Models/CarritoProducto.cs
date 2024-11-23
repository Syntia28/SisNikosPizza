namespace SisNikosPizza.Domain.Models
{
    public class CarritoProducto
    {
        public int CarritoProductoId { get; set; }
        public int ProductoId { get; set; }
        public int CarritoId { get; set; }
        public int Cantidad { get; set; }

        // Navegación a Producto
        public Producto Producto { get; set; }

        // Navegación a Carrito
        public Carrito Carrito { get; set; }
    }
}
