using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Domain.ViewModels;

public class VMDCarrito
{
    public List<CarritoItems> listaCarritoItems { get; set; }
    public Pedido Pedido { get; set; }
    public decimal Total { get; set; }
}
