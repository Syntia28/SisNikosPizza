using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Domain.Models
{
    public class DetallePedido
    {
        public int DetallePedidoId { get; set; }
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
      


        //no database properties
        
        public Pedido Pedido { get; set; }

  
        public Producto Producto { get; set; }

        [NotMapped]
        public float PrecioTotal { get; set; }
       
       
    }
}
