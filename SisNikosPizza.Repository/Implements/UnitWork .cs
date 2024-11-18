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

        public UnitWork(SisNikosPizzaBbContext db)
        {
            _db = db;
            CategoriaRepo = new CategoriaRepository(_db);
            ProductoRepo = new ProductoRepository(_db);

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
    }
}