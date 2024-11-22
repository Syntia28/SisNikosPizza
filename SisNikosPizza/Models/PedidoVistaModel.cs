using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Models
{
    public class PedidoVistaModel
    {
        public PedidoVistaModel() { }
        public List<Producto> ProductosDisponibles { get; set; }
        public List<Pedido> Pedidos { get; set; }
    }
}
