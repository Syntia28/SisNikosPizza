using Maquinarias.Domain.Modelos;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Maquinarias.Domain.ViewModels;

public class VMAccion
{
    public Accion Accion { get; set; }
    // Lista de Categorías
    public IEnumerable<SelectListItem>? DocumentosList { get; set; }
    public IEnumerable<SelectListItem>? UsuariosList { get; set; }
}
