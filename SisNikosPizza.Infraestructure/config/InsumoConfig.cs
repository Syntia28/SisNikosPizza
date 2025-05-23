using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Infraestructure;

public class InsumoConfig : IEntityTypeConfiguration<Insumo>
{
    public void Configure(EntityTypeBuilder<Insumo> builder)
    {
        // Tabla
        builder.ToTable("insumo");

        // Clave primaria
        builder.HasKey(i => i.InsumoId);

        // Propiedades
        builder.Property(i => i.ProveedorId)
            .IsRequired();

        builder.Property(i => i.Nombre)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(i => i.Descripcion)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(i => i.Precio)
            .IsRequired();

        builder.Property(i => i.UnidadeDeMedida)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(i => i.Cantidad)
            .IsRequired();

        builder.Property(i => i.Estado)
            .IsRequired();

        builder.Property(i => i.CreatedAt)
            .IsRequired();
        
        builder.Property(i => i.UpdatedAt)
            .IsRequired();

        // relacion de uno a muchos con DetalleVenta
        builder.HasMany(i => i.ProductoInsumos)
            .WithOne(r => r.Insumo)
            .HasForeignKey(r => r.InsumoId);
    }
}