using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Models
{
    public class NuevoPedidoVista
    {
        public Pedido Pedido { get; set; }
        public List<CarritoProducto> CarritoProductos { get; set; }
    }
}
