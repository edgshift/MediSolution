using Medi.Domain.Core;

namespace Medi.Domain.Entities;

public class Paciente : BaseEntity
{
    public string Nombre { get; set; } = default!;
    public string Apellido { get; set; } = default!;
    public string Cedula { get; set; } = default!;
    public string Telefono { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime FechaNacimiento { get; set; }
    public string Direccion { get; set; } = default!;

    public ICollection<Sesion> Sesiones { get; set; } = new List<Sesion>();
}