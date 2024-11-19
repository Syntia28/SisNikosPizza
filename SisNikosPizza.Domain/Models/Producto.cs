using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Domain.Models
{
    public  class Producto : EntididadBase
    {
        public int ProductoId { get; set; }
        public int Categoriaid { get; set; }
        public Categoria? Category { get; set; }
        public string Nombre { get; set; }
        public string Descripcion {  get; set; }
        public string? ImagenUrl { get; set; }
        public  float Precio { get; set; }
        public string Stock { get; set; }

    }
}
