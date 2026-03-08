using Medi.Domain.Core;

namespace Medi.Domain.Entities;

public class Usuario : BaseEntity
{
    public string Nombre { get; set; } = default!;
    public string Username { get; set; } = default!;
    public string Email { get; set; } = default!;
    public string PasswordHash { get; set; } = default!;
    public string Rol { get; set; } = default!;
}