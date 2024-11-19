using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Domain.Models
{
    public class Proveedor : EntididadBase
    {
        public int ProveedorId { get; set; }
        public string Nombre { get; set; }
        public string Empresa { get; set; }
        public string Producto { get; set; }
        public int Ccantidad { get; set; }

    }
}
