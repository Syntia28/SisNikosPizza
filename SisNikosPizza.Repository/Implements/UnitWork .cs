using SisNikosPizza.Infrastructure.Context;
using SisNikosPizza.Repository.Interfaces;

namespace SisNikosPizza.Repository.Implements
{
    public class UnitWork : IUniwork
    {
        private readonly SisNikosPizzaBbContext _db;

        public ICategoriaRepository CategoriaRepo { get; private set; }
        public ICarritoItemsRepository CarritoItemsRepo { get; private set; }
        public IDetallesPedidoRepository DetallesPedidoRepo { get; private set; }
        public IInsumoRepository InsumoRepo { get; private set; }
        public IPedidoRepository PedidoRepo { get; private set; }
        public IProductoInsumoRepo ProductoInsumoRepo { get; private set; }
        public IProductoRepository ProductoRepo { get; private set; }
        public IProveedorRepository ProveedorRepo { get; private set; }

        public UnitWork(SisNikosPizzaBbContext db)
        {
            _db = db;
            CategoriaRepo = new CategoriaRepository(_db);
            CarritoItemsRepo = new CarritoItemsRepository(_db);
            ProductoRepo = new ProductoRepository(_db);
            ProveedorRepo = new ProveedorRepository(_db);
            PedidoRepo = new PedidoRepository(_db);
            DetallesPedidoRepo = new DetallesPedidoRepository(_db);
            InsumoRepo = new InsumoRepository(_db);
            ProductoInsumoRepo = new ProductoInsumoImpl(_db);

        }
        public void Dispose()
        {
            _db.Dispose();
        }

        public async Task GuardarAsync()
        {
            await _db.SaveChangesAsync();
        }
    }
}