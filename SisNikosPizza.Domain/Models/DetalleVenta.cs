using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Domain.Models
{
    public class DetalleVenta
    {
        public int DetalleVentaId { get; set; }
        public int VentaId { get; set; }
        public int? ProductoId { get; set; }
        public string Descripcion { get; set; }
        public int Cantidad { get; set; }
        public float PrecioUnitario { get; set; }
        public float PrecioTotal { get; set; }
        public Venta Venta { get; set; }
        public Producto? Producto { get; set; }

    }
}
