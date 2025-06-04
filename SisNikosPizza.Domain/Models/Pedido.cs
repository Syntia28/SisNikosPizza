using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

namespace SisNikosPizza.Domain.Models
{
    public class Pedido
    {
        public int PedidoId { get; set; }
        public string UserId { get; set; }
        public IdentityUser? User { get; set; }
        public string EstadoPedido { get; set; } //pendiente, anulado, entregado.
        public string TipoPedido { get; set; } //delivery, recogo_en_local, mesa.

        public bool? Pagado { get; set; }

        //propiedades para delivery.
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Referencia { get; set; }

        //propiedades para recogo en local.
        public DateTime? FechaRecogo { get; set; }

        //propiedades para mesa.
        public string? Mesa { get; set; }//en un futuro se puede cambiar a un objeto mesa.


        public DateTime FechaPedido { get; set; }

        public virtual IEnumerable<DetallePedido>? DetallePedido { get; set; }
     
    }
}
