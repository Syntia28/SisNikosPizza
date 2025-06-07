using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SisNikosPizza.Domain.ViewModels;

public class VMDReportes
{
public int PedidoId { get; set; }
    public DateTime FechaPedido { get; set; }
    public string Cliente { get; set; }
    public int Productos { get; set; }
    public decimal Total   { get; set; }
}
