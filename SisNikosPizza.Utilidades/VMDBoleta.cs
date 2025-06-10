namespace SisNikosPizza.Utilidades;

public class VMDBoleta
{
    public int id { get; set; }
    public DateTime Fecha { get; set; }
    public IEnumerable<VMDDPedidoBoleta> DetallesPedido { get; set; }
    public IEnumerable<VMDDetallesUsuaio> DetallesUsuario { get; set; }
    public double Total { get; set; }
    public string MetodoEntrega { get; set; }
}
