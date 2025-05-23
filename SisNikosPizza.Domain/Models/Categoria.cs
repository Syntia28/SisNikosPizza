using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Domain.Models
{
    public class Categoria : EntididadBase
    {
        public int CategoriaId { get; set; }
        public string nombre { get; set; }

        public IEnumerable<Producto>? producto { get; set; } 

    }
}
