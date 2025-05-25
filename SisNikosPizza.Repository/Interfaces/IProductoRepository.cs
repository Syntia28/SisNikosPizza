using Microsoft.AspNetCore.Mvc.Rendering;
using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Repository.Interfaces
{
    public interface IProductoRepository : IRepositoryBase<Producto>
    {
        void Actualizar(Producto producto);
        IEnumerable<SelectListItem> ListarCategorias(string obj);
        IEnumerable<SelectListItem> ListarInsumos(string obj);
    }
}