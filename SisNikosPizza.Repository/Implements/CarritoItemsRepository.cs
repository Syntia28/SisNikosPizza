using Microsoft.EntityFrameworkCore;
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
    public class CarritoItemsRepository : RepositoryBase<CarritoItems>, ICarritoItemsRepository
    {
        private readonly SisNikosPizzaBbContext _db;
        public CarritoItemsRepository(SisNikosPizzaBbContext db) : base(db)
        {
            _db = db;
        }

        public void ActualizarCantidadCarritoItems(CarritoItems carritoItems)
        {
            var carritoItemDb = _db.carritoItems.FirstOrDefault(c => c.CarritoItemsId == carritoItems.CarritoItemsId);
            if (carritoItemDb != null)
            {
                carritoItemDb.Cantidad = carritoItems.Cantidad;
                carritoItemDb.PrecioTotal = carritoItems.PrecioUnitario * carritoItems.Cantidad;
                _db.SaveChanges();
            }
        }
        public IEnumerable<CarritoItems> ListarCarritoItems(string usuarioId)
        {
            if (usuarioId != null)
            {
                return _db.carritoItems
                    .Include(c => c.Producto)
                    .Include(c => c.Usuario)
                    .Where(c => c.UsuarioId == usuarioId)
                    .ToList();
            }

            return null;
        }
    }
    
}
