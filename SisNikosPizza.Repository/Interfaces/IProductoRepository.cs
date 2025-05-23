using System;
using System.Web.Mvc;
using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Repository.Interfaces
{
    public interface IProductoRepository : IRepositoryBase<Producto>
    {
        void Actualizar(Producto producto);
        Task AgregarInsumoAsync(int productoId, int insumoId, float cantidad);
        Task EliminarInsumoAsync(int productoId, int insumoId);
        Task ActualizarInsumoAsync(int productoId, int insumoId, float nuevaCantidad);
        IEnumerable<SelectListItem> ListarCategorias(string obj);
        IEnumerable<SelectListItem> ListarInsumos(string obj);
    }
}