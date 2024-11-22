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
     
        public DateTime FechaPedido { get; set; }

        public virtual ICollection<DetallePedido> pedidos { get; set; }
        //no database properties
        [NotMapped]
        public IdentityUser Owner { get; set; }
        [NotMapped]
        public float PrecioTotal { get; set; }
        [NotMapped]


    }
}
