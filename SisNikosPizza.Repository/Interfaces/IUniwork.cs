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
        IInsumoRepository InsumoRepo { get; }
        IProveedorRepository ProveedorRepo { get; }
        IPedidoRepository PedidoRepo { get; }
        IProductoInsumoRepo ProductoInsumoRepo { get; }
        ICarritoRepository CarritoRepo { get; }
        IVentaRepository VentaRepo { get; }
        Task GuradarAsync();
    }
}
