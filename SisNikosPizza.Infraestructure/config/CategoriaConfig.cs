using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Infraestructure;

public class CategoriaConfig : IEntityTypeConfiguration<Categoria>
{
    public void Configure(EntityTypeBuilder<Categoria> builder)
    {
        // Tabla
        builder.ToTable("Categoria");

        // Clave primaria
        builder.HasKey(c => c.CategoriaId);

        // Propiedades
        builder.Property(c => c.nombre)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(c => c.Estado)
            .IsRequired();

        builder.Property(c => c.CreatedAt)
            .IsRequired();

        builder.Property(c => c.UpdatedAt)
            .IsRequired();

        // relacion uno a muchos
        builder.HasMany(c => c.producto)
            .WithOne(r => r.categoria)
            .HasForeignKey(r => r.CategoriaId);
    }
}