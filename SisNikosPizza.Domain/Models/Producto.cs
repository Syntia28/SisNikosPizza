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
        public int CategoriaId { get; set; }
        public Categoria? categoria { get; set; }
        public string Nombre { get; set; }
        public string Descripcion {  get; set; }

        //not required field
        public string? ImagenUrl { get; set; }
        public  float Precio { get; set; }
        public string Stock { get; set; }


        //por implementar 
        //proveedor
        // Relación con los productos a través de CarritoProducto
      

        // Relación con los productos a través de CarritoProducto
        public List<ProductoInsumo>? ProductoInsumos { get; set; }
    }
}
