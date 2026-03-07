namespace Medi.Application.DTOs;

public class DoctorDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = default!;
    public string Apellido { get; set; } = default!;
    public string Especialidad { get; set; } = default!;
    public string Telefono { get; set; } = default!;
}