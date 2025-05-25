using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Repository.Interfaces
{
    public interface ICarritoItemsRepository : IRepositoryBase<CarritoItems>
    {
        void ActualizarCantidadCarritoItems(CarritoItems carritoItems);
        IEnumerable<CarritoItems> ListarCarritoItems(string usuarioId);
      
    }
}
