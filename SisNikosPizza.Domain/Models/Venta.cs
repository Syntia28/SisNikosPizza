using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Domain.Models
{
    public class Venta
    {
        public int VentaId { get; set; }
        public DateTime Fecha { get; set; }

        public List<DetalleVenta> Detalles { get; set; }

        //[NotMapped] 
        //public float? ComimsionDelivery { get; set; }

        [NotMapped]
        public float Total
        {
            get
            {
                float total = 0;
                foreach (var detalle in Detalles)
                {
                    total += detalle.PrecioTotal;
                }
                return total;
            }
        }
    }
}
