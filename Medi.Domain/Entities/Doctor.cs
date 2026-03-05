using Medi.Domain.Core;
using Medi.Domain.Entities;

namespace Medi.Domain.Entities;

public class Doctor : BaseEntity
{
    public string Nombre { get; set; } = default!;
    public string Apellido { get; set; } = default!;
    public string Especialidad { get; set; } = default!;
    public string Telefono { get; set; } = default!;

    public ICollection<Sesion> Sesiones { get; set; } = new List<Sesion>();
}