using Microsoft.EntityFrameworkCore;
using SisNikosPizza.Domain.Models;
using SisNikosPizza.Infrastructure.Context;
using SisNikosPizza.Repository.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace SisNikosPizza.Repository.Implements
{
    public class ProductoRepository : RepositoryBase<Producto>, IProductoRepository
    {
        private readonly SisNikosPizzaBbContext _db;

        public ProductoRepository(SisNikosPizzaBbContext db) : base(db)
        {
            _db = db;
        }

        public void Actualizar(Producto producto)
        {
            var productoDB = _db.producto.FirstOrDefault(c => c.ProductoId == producto.ProductoId);
            if (productoDB is not null)
            {
                productoDB.CategoriaId = producto.CategoriaId;
                productoDB.Nombre = producto.Nombre;
                productoDB.Descripcion = producto.Descripcion;
                productoDB.Precio = producto.Precio;
                productoDB.Stock = producto.Stock;
                productoDB.Estado = producto.Estado;

                productoDB.UpdatedAt = DateTime.Now;
                _db.SaveChanges();
            }
        }

        public async Task AgregarInsumoAsync(int productoId, int insumoId, float cantidad)
        {
            var productoInsumo = new ProductoInsumo
            {
                ProductoId = productoId,
                InsumoId = insumoId,
                Cantidad = cantidad
            };

            await _db.ProductoInsumo.AddAsync(productoInsumo);
            await _db.SaveChangesAsync();
        }

        public async Task EliminarInsumoAsync(int productoId, int insumoId)
        {
            var productoInsumo = await _db.ProductoInsumo
                .FirstOrDefaultAsync(pi => pi.ProductoId == productoId && pi.InsumoId == insumoId);

            if (productoInsumo != null)
            {
                _db.ProductoInsumo.Remove(productoInsumo);
                await _db.SaveChangesAsync();
            }
        }

        public async Task ActualizarInsumoAsync(int productoId, int insumoId, float nuevaCantidad)
        {
            var productoInsumo = await _db.ProductoInsumo
                .FirstOrDefaultAsync(pi => pi.ProductoId == productoId && pi.InsumoId == insumoId);

            if (productoInsumo != null)
            {
                productoInsumo.Cantidad = nuevaCantidad;
                await _db.SaveChangesAsync();
            }
        }

        public IEnumerable<SelectListItem> ListarCategorias(string obj)
        {
            if (obj == "Estados")
            {
                return _db.categoria.Where(c => c.EsEstado != VCG.Estado_Activo && c.EsEstado != VCG.Estado_Inactivo).Select(c => new SelectListItem
                {
                    Text = c.EsEstado,
                    Value = c.EstadosId.ToString()
                });

            }

            return null;
        }
        public IEnumerable<SelectListItem> ListarInsumos(string obj)
        {
            if (obj == "Categorias")
            {
                return _db.insumo.Select(c => new SelectListItem
                {
                    Text = c.CtNombre,
                    Value = c.CategoriasId.ToString()
                });
            }

            return null;
        }

    }
}