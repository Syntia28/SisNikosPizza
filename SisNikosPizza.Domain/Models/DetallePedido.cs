using System;

namespace SisNikosPizza.Domain.Models
{
    public class DetallePedido
    {
        public int DetallePedidoId { get; set; }
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public float PrecioTotal { get; set; }


        //relaciones con modelos
        public Pedido? Pedido { get; set; }
        public Producto? Producto { get; set; } 
    }
}
