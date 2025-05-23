using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Infraestructure;

public class DetallePedidoConfig : IEntityTypeConfiguration<DetallePedido>
{
    public void Configure(EntityTypeBuilder<DetallePedido> builder)
    {
        // Tabla
        builder.ToTable("detallePedido");

        // Clave primaria
        builder.HasKey(dp => dp.DetallePedidoId);

        builder.Property(dp => dp.PedidoId)
            .IsRequired();

        builder.Property(dp => dp.ProductoId)
            .IsRequired();

        builder.Property(dp => dp.Cantidad)
            .IsRequired();

        builder.Property(dp => dp.PrecioUnitario)
            .IsRequired();

        builder.Property(dp => dp.PrecioTotal)
            .IsRequired();
    }
}