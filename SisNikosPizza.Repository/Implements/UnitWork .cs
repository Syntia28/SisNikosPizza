using Microsoft.Extensions.Logging;
using SisNikosPizza.Infrastructure.Context;
using SisNikosPizza.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Repository.Implements
{
    public class UnitWork : IUniwork
    {
        private readonly SisNikosPizzaBbContext _db;

        public ICategoriaRepository CategoriaRepo { get; private set; }
        public IProductoRepository ProductoRepo { get; private set; }
        public IInsumoRepository InsumoRepo { get; private set; }
        public IProveedorRepository ProveedorRepo { get; private set; }
        public IPedidoRepository PedidoRepo { get; private set; }
        public IVentaRepository VentaRepo { get; private set; }
        public ICarritoRepository CarritoRepo { get; private set; }
        public UnitWork(SisNikosPizzaBbContext db, 
            ILogger<CarritoRepository> logger
            )
        {
            _db = db;
            CategoriaRepo = new CategoriaRepository(_db);
            ProductoRepo = new ProductoRepository(_db);
            ProveedorRepo = new ProveedorRepository(_db);
            PedidoRepo = new PedidoRepository(_db);
            CarritoRepo = new CarritoRepository(_db, logger);
            VentaRepo = new VentaRepository(_db);
            InsumoRepo = new InsumoRepository(_db);

        }
        public void Dispose()
        {
            _db.Dispose();
        }
       public Task<string?> ObtenerTodosAsync(Func<object, object> ordenarPor)
        {
            throw new NotImplementedException();
        }

        public async Task GuardarCategoria()
        {
           await  _db.SaveChangesAsync();
        }

        public async Task GuardarProducto(){
            await _db.SaveChangesAsync();
        }

        public async Task GuardarInsumo()
        {
            await _db.SaveChangesAsync();
        }

        public async Task GuardarProveedor()
        {
            await _db.SaveChangesAsync();
        }
        public async Task GuardarPedido()
        {
            await _db.SaveChangesAsync();
        }
        public async Task GuardarVenta()
        {
            await _db.SaveChangesAsync();
        }
        public async Task GuardarCarrito()
        {
            await _db.SaveChangesAsync();
        }
    }
}