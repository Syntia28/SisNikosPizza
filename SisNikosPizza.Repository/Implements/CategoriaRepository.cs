using SisNikosPizza.Domain.Models;
using SisNikosPizza.Infrastructure.Context;
using SisNikosPizza.Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Repository.Implements
{
        public class CategoriaRepository : RepositoryBase<Categoria>, ICategoriaRepository
        {
            private readonly SisNikosPizzaBbContext _db;
            public CategoriaRepository(SisNikosPizzaBbContext db) : base(db)
            {
                _db = db;
            }

            public void ActualizarCategoria(Categoria category)
            {
                var categoryDB = _db.categoria.FirstOrDefault(c => c.CategoriaId == category.CategoriaId);
                if (categoryDB is not null)
                {
                    categoryDB.nombre = category.nombre;
                    categoryDB.Estado= category.Estado;
                    categoryDB.UpdatedAt = DateTime.Now;
                    _db.SaveChanges();
                }
            }
        }
    
}
