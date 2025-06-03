using Microsoft.AspNetCore.Identity;
using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Domain.ViewModels;

public class VMDDetallesPedido
{
    public int PedidoId { get; set; }
    public DateTime FechaPedido { get; set; }
    public IdentityUser Usuario { get; set; } // o el tipo correcto de User si usas una clase personalizada
    public IEnumerable<DetallePedido> Detalles { get; set; }
}
