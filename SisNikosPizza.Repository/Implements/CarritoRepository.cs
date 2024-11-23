using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
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


    public class CarritoRepository : RepositoryBase<Carrito>, ICarritoRepository
    {
        private readonly SisNikosPizzaBbContext _db;
        private readonly ILogger<CarritoRepository> _logger;

        public CarritoRepository(SisNikosPizzaBbContext db, ILogger<CarritoRepository> logger) : base(db)
        {
            _db = db;
            _logger = logger;
        }

        public async Task AgregarProductoAlCarrito(string ownerId, int productoId, int cantidad)
        {
           
            var carrito = await _db.carrito
                .Include(c => c.CarritoProductos)
                .FirstOrDefaultAsync(c => c.OwnerId == ownerId);

            if (carrito == null)
            {
                carrito = new Carrito { OwnerId = ownerId, CarritoProductos = new List<CarritoProducto>() };
                await _db.carrito.AddAsync(carrito);
                await _db.SaveChangesAsync();
            }

            var carritoProducto = carrito.CarritoProductos.FirstOrDefault(cp => cp.ProductoId == productoId);

            if (carritoProducto != null)
            {
                carritoProducto.Cantidad += cantidad;
            }
            else
            {
                carritoProducto = new CarritoProducto
                {
                    ProductoId = productoId,
                    CarritoId = carrito.CarritoId,
                    Cantidad = cantidad
                };

                carrito.CarritoProductos.Add(carritoProducto);
                _db.carritoProductos.Add(carritoProducto);
            }

            await _db.SaveChangesAsync();
        }

        public async Task<Carrito> ObtenerCarritoDeUsuario(string ownerId, string baseUrl)
        {
            var carrito = await _db.carrito
                .Include(c => c.CarritoProductos)
                .ThenInclude(cp => cp.Producto)
                .FirstOrDefaultAsync(c => c.OwnerId == ownerId);

            

            if (carrito != null)
            {
                foreach (var carritoProducto in carrito.CarritoProductos)
                {
                    carritoProducto.Producto.ImagenUrl = $"{baseUrl}/{carritoProducto.Producto.ImagenUrl}";
                }
                return carrito;
            }

            carrito = new Carrito { OwnerId = ownerId };
            _db.carrito.Add(carrito);
            await _db.SaveChangesAsync();
            carrito.CarritoProductos = new List<CarritoProducto>();
            return carrito;
        }

        public async Task VaciarCarrito(string ownerId)
        {
            var carrito = await _db.carrito
                .Include(c => c.CarritoProductos)
                .FirstOrDefaultAsync(c => c.OwnerId == ownerId);

            if (carrito != null)
            {
                _db.carritoProductos.RemoveRange(carrito.CarritoProductos);
                _db.carrito.Remove(carrito);
                await _db.SaveChangesAsync();
            }
        }
    }

}
