using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Infraestructure;

public class CarritoItemsConfig : IEntityTypeConfiguration<CarritoItems>
{
    public void Configure(EntityTypeBuilder<CarritoItems> builder)
    {
        builder.ToTable("carritoItems");

        builder.HasKey(c => c.CarritoItemsId);

        builder.Property(c => c.ProductoId)
            .IsRequired();

        builder.Property(c => c.UsuarioId)
            .IsRequired();

        builder.Property(c => c.Cantidad)
            .IsRequired();

        builder.Property(c => c.PrecioUnitario)
            .IsRequired();

        builder.Property(c => c.PrecioTotal)
            .IsRequired();
    }
}