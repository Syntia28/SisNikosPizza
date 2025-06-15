using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Infraestructure;

public class ProveedorConfig : IEntityTypeConfiguration<Proveedor>
{
    public void Configure(EntityTypeBuilder<Proveedor> builder)
    {
        builder.ToTable("proveedor");

        builder.HasKey(p => p.ProveedorId);

        builder.Property(p => p.Nombre)
            .HasMaxLength(50)
            .IsRequired();
        builder.Property(p => p.Empresa)
            .HasMaxLength(100)
            .IsRequired();
        builder.Property(p => p.Cantidad)
            .IsRequired();
        builder.Property(p => p.Estado)
            .IsRequired();
        builder.Property(p => p.CreatedAt)
            .IsRequired();
        builder.Property(p => p.UpdatedAt)
            .IsRequired();

        builder.HasMany(p => p.Insumos)
            .WithOne(i => i.Proveedor)
            .HasForeignKey(i => i.ProveedorId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}