using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SisNikosPizza.Domain.Models;
using System.Reflection;

namespace SisNikosPizza.Infrastructure.Context
{
    public class SisNikosPizzaBbContext : IdentityDbContext
    {
        public SisNikosPizzaBbContext(DbContextOptions<SisNikosPizzaBbContext> options) : base(options)
        { }

        public DbSet<Categoria> categoria { get; set; }
        public DbSet<Producto> producto { get; set; }
        public DbSet<Insumo> insumo { get; set; }
        public DbSet<Proveedor> proveedor { get; set; }
        public DbSet<Pedido> pedido { get; set; }
        public DbSet<Carrito> carrito { get; set; }
        public DbSet<Venta> venta { get; set; }
        public DbSet<CarritoProducto> carritoProductos { get; set; }
        public DbSet<ProductoInsumo> ProductoInsumo { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<DetalleVenta>()
                .HasOne(dv => dv.Producto)
                .WithMany()
                .HasForeignKey(dv => dv.ProductoId)
                .OnDelete(DeleteBehavior.SetNull);

            modelBuilder.Entity<Insumo>()
                .HasOne(i => i.Proveedor)
                .WithMany(p => p.ProveedorInsumo)
                .HasForeignKey(i => i.ProveedorId);

            modelBuilder.Entity<Carrito>().HasMany(s => s.CarritoProductos);
            modelBuilder.Entity<Producto>().HasMany(s => s.ProductoInsumos);
            modelBuilder.Entity<Insumo>().HasMany(pi => pi.Productos);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
