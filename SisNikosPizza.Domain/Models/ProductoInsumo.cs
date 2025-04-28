using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Domain.Models
{

    public class ProductoInsumo
    {
        public int ProductoInsumoId { get; set; }
        public int ProductoId { get; set; }
        public Producto? Producto { get; set; }
        public int InsumoId { get; set; }
        public Insumo? Insumo { get; set; }
        public float Cantidad { get; set; }
    }
}
