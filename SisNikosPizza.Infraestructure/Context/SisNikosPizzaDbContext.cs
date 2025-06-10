using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SisNikosPizza.Domain.Models;
using System.Reflection;

namespace SisNikosPizza.Infrastructure.Context
{
    public class SisNikosPizzaBbContext : IdentityDbContext
    {
        public SisNikosPizzaBbContext(DbContextOptions<SisNikosPizzaBbContext> options) : base(options)
        {}

        public DbSet<Categoria> categoria { get; set; }
        public DbSet<CarritoItems> carritoItems { get; set; }
        public DbSet<Producto> producto { get; set; }
        public DbSet<Insumo> insumo { get; set; }
        public DbSet<Proveedor> proveedores { get; set; }
        public DbSet<Pedido> pedido { get; set; }
        public DbSet<DetallePedido> detallePedido { get; set; }
        public DbSet<ProductoInsumo> ProductoInsumo { get; set; }
        public DbSet<ApplicationUser> Users { get; set; }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
