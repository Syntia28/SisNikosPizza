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
        Task <List<Pedido>> ObtenerPedidos(String baseUrl);

    }
}
