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
        public string OwnerId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public DateTime FechaPedido { get; set; }


        //no database properties
        [NotMapped]
        public Pedido Pedido { get; set; }

        [NotMapped]
        public Producto Producto { get; set; }
        [NotMapped]
        public IdentityUser Owner { get; set; }
        [NotMapped]
        public float PrecioTotal { get; set; }
        [NotMapped]
        public float PrecioUnitario { get; set; }
        [NotMapped]
        public string NombreProducto { get; set; }
        [NotMapped]
        public string NombreUsuario { get; set; }
        [NotMapped]
        public String ImagenUrl { get; set; }
    }
}
