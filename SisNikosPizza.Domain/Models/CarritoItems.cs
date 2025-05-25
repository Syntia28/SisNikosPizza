using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SisNikosPizza.Domain.Models;

public class CarritoItems
{
    public int CarritoItemsId { get; set; }
    public int ProductoId { get; set; }
    public virtual Producto? Producto { get; set; }
    public string UsuarioId { get; set; }
    public virtual IdentityUser? Usuario { get; set; }
    public int Cantidad { get; set; }
    public decimal PrecioUnitario { get; set; }
    public decimal PrecioTotal { get; set; }
}