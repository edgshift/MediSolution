using System.ComponentModel.DataAnnotations;

namespace Medi.Application.DTOs;

public class SesionDto
{
    public int Id { get; set; }

    [Required]
    public DateTime FechaHora { get; set; }

    [Range(1, 1440)]
    public int DuracionMinutos { get; set; }

    [StringLength(500)]
    public string? Notas { get; set; }

    [Range(1, int.MaxValue)]
    public int PacienteId { get; set; }

    [Range(1, int.MaxValue)]
    public int DoctorId { get; set; }

    [Range(1, int.MaxValue)]
    public int TratamientoId { get; set; }
}