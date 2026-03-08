using System.ComponentModel.DataAnnotations;

namespace Medi.Application.DTOs;

public class RegisterUserDto
{
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = default!;

    [Required]
    [StringLength(50)]
    public string Username { get; set; } = default!;

    [Required]
    [EmailAddress]
    [StringLength(150)]
    public string Email { get; set; } = default!;

    [Required]
    [MinLength(6)]
    public string Password { get; set; } = default!;

    [Required]
    [RegularExpression("^(Admin|Doctor|Recepcion)$", ErrorMessage = "Rol debe ser Admin, Doctor o Recepcion.")]
    public string Rol { get; set; } = default!;
}
