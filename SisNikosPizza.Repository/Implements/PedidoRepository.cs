using Azure.Core;
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


    public class PedidoRepository : RepositoryBase<Pedido>, IPedidoRepository
    {
        private readonly SisNikosPizzaBbContext _db;
        public PedidoRepository(SisNikosPizzaBbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<List<Pedido>> ObtenerPedidosDetallados(string ownerId)
        {
            return await _db.pedido
                .Include(p => p.DetallePedidos)
                .ThenInclude(pp => pp.Producto)
                .Where(p => p.OwnerId == ownerId)
                .ToListAsync();
        }
        public async Task<List<Pedido>> ObtenerPedidosDetalladosTodos()
        {
            return await _db.pedido
                .Include(p => p.DetallePedidos)
                .ThenInclude(pp => pp.Producto)
                .ToListAsync();
        }
        public async Task<Pedido> ObtenerPedidoDetallado(int pedidoId, string baseUrl)
        {
            var pedido = await _db.pedido
                .Include(p => p.DetallePedidos)
                .ThenInclude(pp => pp.Producto)
                .Where(p => p.PedidoId == pedidoId)
                .FirstOrDefaultAsync();

            if (pedido != null)
            {
                foreach (var detalle in pedido.DetallePedidos)
                {
                    if (detalle.Producto != null && !string.IsNullOrEmpty(detalle.Producto.ImagenUrl))
                    {
                        detalle.Producto.ImagenUrl = $"{baseUrl}/{detalle.Producto.ImagenUrl}";
                    }
                }
            }

            return pedido;
        }
    }

}
