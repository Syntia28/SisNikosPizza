using SisNikosPizza.Infrastructure.Context;
using SisNikosPizza.Repository.Interfaces;
using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Repository.Implements
{
    public class ProductoInsumoImpl : RepositoryBase<ProductoInsumo>, IProductoInsumoRepo
    {
        private readonly SisNikosPizzaBbContext _context;

        public ProductoInsumoImpl(SisNikosPizzaBbContext context) : base(context)
        {
            _context = context;
        }

        public void Actualizar(ProductoInsumo insumo)
        {
            
        }
    }
}
