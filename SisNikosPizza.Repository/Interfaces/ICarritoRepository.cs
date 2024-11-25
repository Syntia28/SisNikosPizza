using SisNikosPizza.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Repository.Interfaces
{
    public interface ICarritoRepository : IRepositoryBase<Carrito>
    {
        public Task VaciarCarrito (string ownerId);
        public Task<Carrito> ObtenerCarritoDeUsuario(string ownerId, string baseUrl);
       public Task AgregarProductoAlCarrito(string ownerId, int productoId, int cantidad);
        public Task<int> ObtenerTotalCarrito(string ownerId);
        public Task EliminarProductoDeCarrito(string ownerId, int productoId);
        public Task IncrementarCantidadProducto(string ownerId, int productoId);
        public Task DecrementarCantidadProducto(string ownerId, int productoId);
    }
}
