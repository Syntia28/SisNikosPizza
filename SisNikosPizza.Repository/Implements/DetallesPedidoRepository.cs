using Azure.Core;
using Microsoft.EntityFrameworkCore;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Domain.ViewModels;
using SisNikosPizza.Infrastructure.Context;
using SisNikosPizza.Repository.Interfaces;
using SisNikosPizza.Utilidades;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Repository.Implements
{


    public class DetallesPedidoRepository : RepositoryBase<DetallePedido>, IDetallesPedidoRepository
    {
        private readonly SisNikosPizzaBbContext _db;
        public DetallesPedidoRepository(SisNikosPizzaBbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<VMDDetallesPedido> GetDetallesByPedidoIdAsync(int pedidoId)
        {
            var pedidosDelivery = await _db.detallePedido
            .Where(p => p.Pedido.PedidoId == pedidoId)
            .Include(p => p.Pedido)
            .ThenInclude(p => p.User)
            .Include(p => p.Producto)
            .OrderByDescending(p => p.Pedido.FechaPedido)
            .ToListAsync();

            return new VMDDetallesPedido
            {
                PedidoId = pedidosDelivery.First().Pedido.PedidoId,
                FechaPedido = pedidosDelivery.First().Pedido.FechaPedido,
                Usuario = pedidosDelivery.First().Pedido.User,
                Detalles = pedidosDelivery
            };
        }

        public async Task<IEnumerable<VMDDetallesPedido>> GetDeliveryPedidosAsync()
        {
            var pedidosDelivery = await _db.detallePedido
                .Where(p => p.Pedido.TipoPedido == VCG.TipoPedido.Delivery && p.Pedido.Pagado == false)
                .Include(p => p.Pedido)
                .ThenInclude(p => p.User)
                .Include(p => p.Producto)
                .OrderByDescending(p => p.Pedido.FechaPedido)
                .ToListAsync();

            return pedidosDelivery
                .GroupBy(p => p.PedidoId)
                .Select(grupo => new VMDDetallesPedido
                {
                    PedidoId = grupo.Key,
                    FechaPedido = grupo.First().Pedido.FechaPedido,
                    Usuario = grupo.First().Pedido.User,
                    Detalles = grupo.ToList()
                }).ToList();
        }

        public async Task<IEnumerable<VMDDetallesPedido>> GetPedidosAsync()
        {
            var pedidosDelivery = await _db.detallePedido
                .Where(p => p.Pedido.Pagado == false)
                .Include(p => p.Pedido)
                .ThenInclude(p => p.User)
                .Include(p => p.Producto)
                .OrderByDescending(p => p.Pedido.FechaPedido)
                .ToListAsync();

            return pedidosDelivery
                .GroupBy(p => p.PedidoId)
                .Select(grupo => new VMDDetallesPedido
                {
                    PedidoId = grupo.Key,
                    FechaPedido = grupo.First().Pedido.FechaPedido,
                    Usuario = grupo.First().Pedido.User,
                    Detalles = grupo.ToList()
                }).ToList();
        }

        public async Task<IEnumerable<VMDDetallesPedido>> GetVentasAsync()
        {
            var pedidosDelivery = await _db.detallePedido
                .Where(p => p.Pedido.Pagado == true)
                .Include(p => p.Pedido)
                .ThenInclude(p => p.User)
                .Include(p => p.Producto)
                .OrderByDescending(p => p.Pedido.FechaPedido)
                .ToListAsync();

            return pedidosDelivery
                .GroupBy(p => p.PedidoId)
                .Select(grupo => new VMDDetallesPedido
                {
                    PedidoId = grupo.Key,
                    FechaPedido = grupo.First().Pedido.FechaPedido,
                    Usuario = grupo.First().Pedido.User,
                    Detalles = grupo.ToList()
                }).ToList();
        }
    }

}
