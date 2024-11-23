using System;
using System.Collections.Generic;
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
    }
}
