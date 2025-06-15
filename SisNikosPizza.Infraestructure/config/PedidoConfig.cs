using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Infraestructure;

public class PedidoConfig : IEntityTypeConfiguration<Pedido>
{
    public void Configure(EntityTypeBuilder<Pedido> builder)
    {
        builder.ToTable("pedido");

        builder.HasKey(pe => pe.PedidoId);

        builder.Property(pe => pe.UserId)
            .IsRequired();

        builder.Property(pe => pe.EstadoPedido)
            .IsRequired();

        builder.Property(pe => pe.TipoPedido)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(pe => pe.Pagado)
             .IsRequired(false);

        builder.Property(pe => pe.Telefono)
            .HasMaxLength(20)
            .IsRequired(false);

        builder.Property(pe => pe.Direccion)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(pe => pe.Referencia)
            .HasMaxLength(100)
            .IsRequired(false);

        builder.Property(pe => pe.FechaRecogo)
            .IsRequired(false);

        builder.Property(pe => pe.Mesa)
            .HasMaxLength(10)
            .IsRequired(false);

        builder.Property(pe => pe.FechaPedido)
            .IsRequired();

        builder.HasMany(pe => pe.DetallePedido)
            .WithOne(r => r.Pedido)
            .HasForeignKey(r => r.PedidoId);
    }
}