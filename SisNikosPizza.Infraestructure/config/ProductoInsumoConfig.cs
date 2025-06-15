using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Infraestructure;

public class ProductoInsumoConfig : IEntityTypeConfiguration<ProductoInsumo>
{
    public void Configure(EntityTypeBuilder<ProductoInsumo> builder)
    {
        builder.ToTable("ProductoInsumo");

        builder.HasKey(pi => pi.ProductoInsumoId);

        builder.Property(pi => pi.ProductoId)
            .IsRequired();

        builder.Property(pi => pi.InsumoId)
            .IsRequired();

        builder.Property(pi => pi.Cantidad)
            .IsRequired();
    }
}