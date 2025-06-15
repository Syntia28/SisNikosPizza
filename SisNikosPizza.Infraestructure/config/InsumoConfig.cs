using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Infraestructure;

public class InsumoConfig : IEntityTypeConfiguration<Insumo>
{
    public void Configure(EntityTypeBuilder<Insumo> builder)
    {
        builder.ToTable("insumo");

        builder.HasKey(i => i.InsumoId);

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
        builder.HasMany(i => i.ProductoInsumos)
            .WithOne(r => r.Insumo)
            .HasForeignKey(r => r.InsumoId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}