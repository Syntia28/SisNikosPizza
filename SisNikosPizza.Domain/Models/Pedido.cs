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
        public string EstadoPedido { get; set; } 
        public string TipoPedido { get; set; } 

        public bool? Pagado { get; set; }

        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Referencia { get; set; }

        public DateTime? FechaRecogo { get; set; }

        public string? Mesa { get; set; }
        public DateTime FechaPedido { get; set; }

        public virtual IEnumerable<DetallePedido>? DetallePedido { get; set; }
     
    }
}
