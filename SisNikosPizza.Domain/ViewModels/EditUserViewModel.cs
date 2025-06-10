using System.ComponentModel.DataAnnotations;

namespace SisNikosPizza.Domain.ViewModels;

public class EditUserViewModel
{
    public string Id { get; set; }

    [Required(ErrorMessage = "El nombre de usuario es obligatorio.")]
    [Display(Name = "Nombre de Usuario")]
    public string UserName { get; set; }

    [Required(ErrorMessage = "El correo electrónico es obligatorio.")]
    [EmailAddress(ErrorMessage = "El formato del correo electrónico no es válido.")]
    public string Email { get; set; }

    [Phone(ErrorMessage = "El formato del número de teléfono no es válido.")]
    [Display(Name = "Número de Teléfono")]
    public string PhoneNumber { get; set; }
}
