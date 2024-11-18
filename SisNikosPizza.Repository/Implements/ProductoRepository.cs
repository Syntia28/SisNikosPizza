using SisNikosPizza.Domain.Models;
using SisNikosPizza.Infrastructure.Context;
using SisNikosPizza.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;


namespace SisNikosPizza.Repository.Implements
{
    public class ProductoRepository : RepositoryBase<Producto>, IProductoRepository
    {
        private readonly SisNikosPizzaBbContext _db;
        public ProductoRepository(SisNikosPizzaBbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Producto producto)
        {
            var productoDB = _db.producto.FirstOrDefault(c => c.ProductoId == producto.ProductoId);
            if (productoDB is not null)
            {

                productoDB.Categoriaid = producto.Categoriaid;
                productoDB.Nombre = producto.Nombre;
                productoDB.Descripcion = producto.Descripcion;
                productoDB.Precio = producto.Precio;
                productoDB.Stock = producto.Stock;
                productoDB.Estado = producto.Estado;

                productoDB.UpdatedAt = DateTime.Now;
                _db.SaveChanges();
            }
        }

      
    }
}
