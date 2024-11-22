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

     public async Task<List<Pedido>> ObtenerPedidos(String baseUrl)
        {
            var pedidos = await _db.pedido
           .Include(p => p.Producto)
           .Include(p => p.Owner)
           .ToListAsync();

            var describedPedidos = new List<Pedido>();
            foreach (var pedido in pedidos)
            {
            
                string imagenUrl = string.Empty;
                if (!string.IsNullOrEmpty(pedido.Producto.ImagenUrl))
                {
                    imagenUrl = $"{baseUrl}/{pedido.Producto.ImagenUrl}";
                }

                var describedPedido = new Pedido
                {
                    PedidoId = pedido.PedidoId,
                    Cantidad = pedido.Cantidad,

                    FechaPedido = pedido.FechaPedido,
                   NombreUsuario = pedido.Owner.Email,
                    NombreProducto = pedido.Producto.Nombre,

                    PrecioTotal = pedido.Producto.Precio * (float)pedido.Cantidad,
                    ImagenUrl = imagenUrl



                };
                describedPedidos.Add(describedPedido);
            }
            return describedPedidos;
        }
    }

}
