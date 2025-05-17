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
    public class InsumoRepository : RepositoryBase<Insumo>, IInsumoRepository
    {
        private readonly SisNikosPizzaBbContext _db;
        public InsumoRepository(SisNikosPizzaBbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Insumo insumo)
        {
            var insumoDB = _db.insumo.FirstOrDefault(c => c.InsumoId == insumo.InsumoId);
            if (insumoDB is not null)
            {
                insumo.UpdatedAt = DateTime.Now;

                insumoDB.Nombre = insumo.Nombre;
                insumoDB.Descripcion = insumo.Descripcion;
                insumoDB.Precio = insumo.Precio;
                insumoDB.UnidadeDeMedida = insumo.UnidadeDeMedida;
                insumoDB.Cantidad = insumo.Cantidad;
                insumoDB.Estado = insumo.Estado;

                _db.SaveChanges();
            }
        }


    }
}
