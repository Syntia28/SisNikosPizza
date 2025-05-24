using Microsoft.AspNetCore.Mvc.Rendering;
using SisNikosPizza.Domain.Models;

namespace SisNikosPizza.Domain.ViewModels;

public class VMDProducto
{
    public Producto producto { get; set; }
    public IEnumerable<SelectListItem>? CategoriasList { get; set; }
    public IEnumerable<SelectListItem>? InsumosList { get; set; }
}
