
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SisNikosPizza.Domain.Models;
using System.Collections.Generic;
using System.Reflection;
using System.Reflection.Emit;


namespace SisNikosPizza.Infrastructure.Context;

public class SisNikosPizzaBbContext : IdentityDbContext
{
    public SisNikosPizzaBbContext(DbContextOptions<SisNikosPizzaBbContext> options) : base(options)
    { }

    // Agregar los modelos que van a mapear a la base de datos
    public DbSet<Categoria> categoria { get; set; }
    public DbSet<Producto> producto { get; set; }
    public DbSet<Proveedor> proveedor { get; set; }
    public DbSet<Pedido> pedido { get; set; }
    public DbSet<Carrito> carrito { get; set; }
    public DbSet<Venta> venta { get; set; }
    public DbSet<CarritoProducto> carritoProductos { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        //FluentAPI tiene mayor prevalencia que Data Annotations

        // Fluent API para categorías
        //modelBuilder.Entity<Category>().ToTable("Category");
        //modelBuilder.Entity<Category>()
        //    .HasKey(x => x.CategoryId);
        //modelBuilder.Entity<Category>()
        //    .Property(x => x.Name)
        //    .HasMaxLength(60)
        //    .IsRequired()
        //    .IsUnicode();

        //relaciones carrito

        

        // Relación Carrito -> CarritoProducto
        modelBuilder.Entity<Carrito>().HasMany(s => s.CarritoProductos);




        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}