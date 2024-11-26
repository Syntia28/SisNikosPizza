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


    public class VentaRepository : RepositoryBase<Venta>, IVentaRepository
    {
        private readonly SisNikosPizzaBbContext _db;
        public VentaRepository(SisNikosPizzaBbContext db) : base(db)
        {
            _db = db;
        }
        public async Task<List<Venta>> ObtenerVentasDetallados()
        {
            return await _db.venta
                .Include(v => v.Detalles)
                .ThenInclude(vp => vp.Producto).Include(v => v.Owner)

                .ToListAsync();
        }
       
        public async Task<Venta> ObtenerVentaDetallado(int ventaId, string baseUrl)
        {
            var venta = await _db.venta
                .Include(v => v.Detalles)
                .ThenInclude(vp => vp.Producto).Include(v => v.Owner)
                .Where(v => v.VentaId == ventaId)
                .FirstOrDefaultAsync();

            if (venta != null)
            {
                foreach (var detalle in venta.Detalles)
                {
                    if (detalle.Producto != null && !string.IsNullOrEmpty(detalle.Producto.ImagenUrl))
                    {
                        detalle.Producto.ImagenUrl = $"{baseUrl}/{detalle.Producto.ImagenUrl}";
                    }
                }
            }

            return venta;
        }
    }

}
