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
        IDetallesPedidoRepository DetallesPedidoRepo { get; }
        IInsumoRepository InsumoRepo { get; }
        IPedidoRepository PedidoRepo { get; }
        IProductoInsumoRepo ProductoInsumoRepo { get; }
        IProductoRepository ProductoRepo { get; }
        IProveedorRepository ProveedorRepo { get; }
        IVentaRepository VentaRepo { get; }
        Task GuradarAsync();
    }
}
