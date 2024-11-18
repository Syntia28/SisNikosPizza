using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Repository.Interfaces
{
    public  interface IUniwork : IDisposable
    {
        ICategoriaRepository CategoriaRepo { get; }
        IProductoRepository ProductoRepo { get; }
       

        Task GuardarCategoria();

        Task GuardarProducto();
        
    }
}
