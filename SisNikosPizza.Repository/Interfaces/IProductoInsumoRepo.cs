using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Repository.Interfaces
{
    public interface IProductoInsumoRepo : IRepositoryBase<ProductoInsumo>
    {
        public void Actualizar(ProductoInsumo insumo);
    }
}
