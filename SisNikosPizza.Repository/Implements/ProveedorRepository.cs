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
    public class ProveedorRepository : RepositoryBase<Proveedor>, IProveedorRepository
    {
        private readonly SisNikosPizzaBbContext _db;
        public ProveedorRepository(SisNikosPizzaBbContext db) : base(db)
        {
            _db = db;
        }

        public void ActualizarProveedor(Proveedor proveedor)
        {
            var proveedorDB = _db.proveedores.FirstOrDefault(c => c.ProveedorId == proveedor.ProveedorId);
            if (proveedorDB is not null)
            {
                proveedorDB.Nombre = proveedor.Nombre;
                proveedorDB.Empresa = proveedor.Empresa;
                proveedorDB.Ccantidad = proveedor.Ccantidad;
               
                proveedorDB.UpdatedAt = DateTime.Now;
                _db.SaveChanges();
            }

        }
        public async Task<List<Insumo>> ObtenerInsumosPorProveedorAsync(int proveedorId)
        {
            return await _db.insumo
                .Where(i => i.ProveedorId == proveedorId)
                .ToListAsync();
        }

        public async Task<Proveedor?> ObtenerProveedorConInsumosAsync(int proveedorId)
        {
            return await _db.proveedores
                .Include(p => p.ProveedorInsumo) 
                .FirstOrDefaultAsync(p => p.ProveedorId == proveedorId);
        }
    }
}
