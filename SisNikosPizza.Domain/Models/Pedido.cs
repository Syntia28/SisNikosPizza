using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Domain.Models
{
    public class Pedido : EntididadBase
    {
        public int PedidoId { get; set; }
        public string OwnerId { get; set; }

        public string EstadoPedido { get; set; } //pendiente, anulado, entregado.
        public string TipoPedido { get; set; } //delivery, recogo_en_local, mesa.

        //propiedades para delivery.

        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public string? Referencia { get; set; }


        //propiedades para recogo en local.
        public DateTime? FechaRecogo { get; set; }

        //propiedades para mesa.
        public string? Mesa { get; set; }//en un futuro se puede cambiar a un objeto mesa.


        public DateTime FechaPedido { get; set; }

        public virtual ICollection<DetallePedido> DetallePedidos { get; set; }
        //no database properties
        [NotMapped]
        public IdentityUser Owner { get; set; }
        [NotMapped]
        public float PrecioTotal { get; set; }
       


    }
}
