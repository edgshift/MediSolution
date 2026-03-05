using Medi.Domain.Core;

namespace Medi.Domain.Entities;

public class Sesion : BaseEntity
{
    public DateTime FechaHora { get; set; }
    public int DuracionMinutos { get; set; }
    public string? Notas { get; set; }

    public int PacienteId { get; set; }
    public int DoctorId { get; set; }
    public int TratamientoId { get; set; }

    public Paciente? Paciente { get; set; }
    public Doctor? Doctor { get; set; }
    public Tratamiento? Tratamiento { get; set; }
}