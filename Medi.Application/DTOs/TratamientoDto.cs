using System.ComponentModel.DataAnnotations;

namespace Medi.Application.DTOs;

public class TratamientoDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = default!;

    [Required]
    [StringLength(500)]
    public string Descripcion { get; set; } = default!;

    [Range(0, 999999999)]
    public decimal Costo { get; set; }
}