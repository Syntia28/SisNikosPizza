using SisNikosPizza.Domain.Models;
using SisNikosPizza.Domain.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Repository.Interfaces
{
    public interface IDetallesPedidoRepository : IRepositoryBase<DetallePedido>
    {
        Task<VMDDetallesPedido> GetDetallesByPedidoIdAsync(int pedidoId);
        Task<IEnumerable<VMDDetallesPedido>> GetDeliveryPedidosAsync();
        Task<IEnumerable<VMDDetallesPedido>> GetPedidosAsync();
        Task<IEnumerable<VMDDetallesPedido>> GetVentasAsync();
    }
}
