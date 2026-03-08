namespace Medi.Application.DTOs;

public class AuthResponseDto
{
    public string Token { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Rol { get; set; } = default!;
    public string Nombre { get; set; } = default!;
}