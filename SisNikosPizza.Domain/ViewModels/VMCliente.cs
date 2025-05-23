using Maquinarias.Domain.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Maquinarias.Domain.ViewModels;

public class VMCliente
{
    public Cliente Cliente { get; set; }
    // Lista de Categorías
    public IEnumerable<SelectListItem>? EstadosList { get; set; }
}
