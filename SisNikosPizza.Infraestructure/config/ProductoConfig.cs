using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Infraestructure;

public class ProductoConfig : IEntityTypeConfiguration<Producto>
{
    public void Configure(EntityTypeBuilder<Producto> builder)
    {
        // Tabla
        builder.ToTable("Producto");

        // Clave primaria
        builder.HasKey(p => p.ProductoId);

        // Propiedades
        builder.Property(P => P.CategoriaId)
            .IsRequired();

        builder.Property(p => p.Nombre)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Descripcion)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(p => p.ImagenUrl)
            .HasMaxLength(200)
            .IsRequired(false);

        builder.Property(p => p.Precio)
            .IsRequired();

        builder.Property(p => p.Stock)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(p => p.Estado)
            .IsRequired();
        builder.Property(p => p.CreatedAt)
            .IsRequired();
        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        // relacion uno a muchos
        builder.HasMany(p => p.ProductoInsumos)
            .WithOne(i => i.Producto)
            .HasForeignKey(i => i.ProductoId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasMany(p => p.DetallePedidos)
            .WithOne(i => i.Producto)
            .HasForeignKey(i => i.ProductoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}