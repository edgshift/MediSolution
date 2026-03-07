namespace Medi.Application.DTOs;

public class TratamientoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Costo { get; set; }
}