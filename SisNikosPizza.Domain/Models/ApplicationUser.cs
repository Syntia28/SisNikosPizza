using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SisNikosPizza.Domain.Models;

public class ApplicationUser : IdentityUser
{
    [Required(ErrorMessage = "Nombres es requerido")]
    [MaxLength(100)]
    public string Nombre { get; set; }

   
    public string? Direccion { get; set; }
    public DateTime? FechaNacimiento { get; set; }

    
    [NotMapped] 
    public string Role { get; set; }
}
