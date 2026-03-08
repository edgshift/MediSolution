using System.ComponentModel.DataAnnotations;

namespace Medi.Application.DTOs;

public class LoginDto
{
    [Required]
    public string UsernameOrEmail { get; set; } = default!;

    [Required]
    public string Password { get; set; } = default!;
}