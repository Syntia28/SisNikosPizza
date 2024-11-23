using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Models
{
    public class CobrarPedidoModel
    {
        public Pedido Pedido { get; set; }
        public Venta Venta { get; set; }
        public string? ComisionDelivery { get; set; }
    }
}
