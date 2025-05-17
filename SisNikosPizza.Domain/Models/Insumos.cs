using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Domain.Models
{
    public class Insumo : EntididadBase
    {
        public int InsumoId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }

        public float Precio { get; set; }
        public string UnidadeDeMedida { get; set; }
        public float Cantidad { get; set; }

        public List<ProductoInsumo>? Productos { get; set; }

    }
}
