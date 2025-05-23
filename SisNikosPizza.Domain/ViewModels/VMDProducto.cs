using SisNikosPizza.Domain.Models;
using System.Web.Mvc;

namespace Maquinarias.Domain.ViewModels;

public class VMDProducto
{
    public Producto producto { get; set; }
    // Lista de Categorías
    public IEnumerable<SelectListItem>? EstadosList { get; set; }
    public IEnumerable<SelectListItem>? CategoriaList { get; set; }
    public IEnumerable<SelectListItem>? ClientesList { get; set; }
}
