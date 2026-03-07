namespace Medi.Application.DTOs;

public class PacienteDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = default!;
    public string Apellido { get; set; } = default!;
    public string Cedula { get; set; } = default!;
    public string Telefono { get; set; } = default!;
    public string Email { get; set; } = default!;
    public DateTime FechaNacimiento { get; set; }
    public string Direccion { get; set; } = default!;
}