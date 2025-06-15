using SisNikosPizza.Domain.Models;
using SisNikosPizza.Infrastructure.Context;
using SisNikosPizza.Utilidades; 
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace F_M_Maquinarias.Infrastructure.Data;

public static class SeedData
{
    public static void Initialize(IServiceProvider serviceProvider)
    {
        using (var context = new SisNikosPizzaBbContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<SisNikosPizzaBbContext>>()))
        {
            context.Database.Migrate();

            if (!context.categoria.Any())
            {
                context.categoria.AddRange(
                    new Categoria { nombre = "cat1" },
                    new Categoria { nombre = "cat2" },
                    new Categoria { nombre = "cat3" },
                    new Categoria { nombre = "cat4" },
                    new Categoria { nombre = "cat5" },
                    new Categoria { nombre = "cat6" },
                    new Categoria { nombre = "cat7" }
                );
                context.SaveChanges();
            }

            if (!context.proveedores.Any())
            {
                context.proveedores.AddRange(
                    new Proveedor { Nombre = "proveedor1", Empresa = "empresa1", Cantidad = "100" },
                    new Proveedor { Nombre = "proveedor2", Empresa = "empresa2", Cantidad = "200" },
                    new Proveedor { Nombre = "proveedor3", Empresa = "empresa3", Cantidad = "300" },
                    new Proveedor { Nombre = "proveedor4", Empresa = "empresa4", Cantidad = "400" },
                    new Proveedor { Nombre = "proveedor5", Empresa = "empresa5", Cantidad = "500" },
                    new Proveedor { Nombre = "proveedor6", Empresa = "empresa6", Cantidad = "600" },
                    new Proveedor { Nombre = "proveedor7", Empresa = "empresa7", Cantidad = "700" }
                );
                context.SaveChanges();
            }

            if (!context.insumo.Any())
            {
                var proveedores = context.proveedores.ToList();

                context.insumo.AddRange(
                    new Insumo
                    {
                        ProveedorId = proveedores[0].ProveedorId,
                        Nombre = "Insumo 1",
                        Descripcion = "Descripción del insumo 1",
                        Precio = 10.5f,
                        UnidadeDeMedida = "kg",
                        Cantidad = 100,
                        Estado = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Insumo
                    {
                        ProveedorId = proveedores[1].ProveedorId,
                        Nombre = "Insumo 2",
                        Descripcion = "Descripción del insumo 2",
                        Precio = 20.0f,
                        UnidadeDeMedida = "L",
                        Cantidad = 50,
                        Estado = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    },
                    new Insumo
                    {
                        ProveedorId = proveedores[2].ProveedorId,
                        Nombre = "Insumo 3",
                        Descripcion = "Descripción del insumo 3",
                        Precio = 5.25f,
                        UnidadeDeMedida = "unidad",
                        Cantidad = 200,
                        Estado = true,
                        CreatedAt = DateTime.Now,
                        UpdatedAt = DateTime.Now
                    }
                );
                context.SaveChanges();
            }

        }
    }
}