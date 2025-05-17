using SisNikosPizza.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace SisNikosPizza.Repository.Interfaces
{
    public interface IProductoRepository : IRepositoryBase<Producto>
    {
        void Actualizar(Producto producto);
        Task AgregarInsumoAsync(int productoId, int insumoId, float cantidad);
        Task EliminarInsumoAsync(int productoId, int insumoId);
        Task ActualizarInsumoAsync(int productoId, int insumoId, float nuevaCantidad);
    }
}