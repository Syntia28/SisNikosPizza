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
        Task GuardarCategoria();
        IProductoRepository ProductoRepo { get; }
        Task GuardarProducto();
        IInsumoRepository InsumoRepo { get; }
        Task GuardarInsumo();
        IProveedorRepository ProveedorRepo { get; }
        Task GuardarProveedor();
        IPedidoRepository PedidoRepo { get; }
        Task GuardarPedido();

        IProductoInsumoRepo ProductoInsumoRepo { get; }
        Task GuardarProductoInsumo();

        ICarritoRepository CarritoRepo { get; }

        Task GuardarCarrito();

        IVentaRepository VentaRepo { get; }
        Task GuardarVenta();
    }
}
