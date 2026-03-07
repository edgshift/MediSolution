namespace Medi.Application.DTOs;

public class SesionDto
{
    public int Id { get; set; }
    public DateTime FechaHora { get; set; }
    public int DuracionMinutos { get; set; }
    public string? Notas { get; set; }
    public int PacienteId { get; set; }
    public int DoctorId { get; set; }
    public int TratamientoId { get; set; }
}