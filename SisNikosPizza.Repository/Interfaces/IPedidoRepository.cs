using SisNikosPizza.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Repository.Interfaces
{
    public interface IPedidoRepository : IRepositoryBase<Pedido>
    {

        public Task<List<Pedido>> ObtenerPedidosDetallados(string UserId);
        public Task<List<Pedido>> ObtenerPedidosDetalladosTodos();

        public Task<Pedido> CobrarPedido(int pedidoId, string UserId);

        public Task<Pedido> ObtenerPedidoDetallado(int pedidoId, string baseUrl);
    }
}
