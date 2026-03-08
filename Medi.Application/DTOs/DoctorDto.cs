using System.ComponentModel.DataAnnotations;

namespace Medi.Application.DTOs;

public class DoctorDto
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = default!;

    [Required]
    [StringLength(100)]
    public string Apellido { get; set; } = default!;

    [Required]
    [StringLength(20)]
    [RegularExpression(@"^[0-9\-]+$", ErrorMessage = "La cédula solo puede contener números y guiones.")]
    public string Cedula { get; set; } = default!;

    [Required]
    [StringLength(20)]
    [RegularExpression("^(Masculino|Femenino)$", ErrorMessage = "Sexo debe ser Masculino o Femenino.")]
    public string Sexo { get; set; } = default!;

    [Required]
    [StringLength(100)]
    public string Especialidad { get; set; } = default!;

    [Required]
    [Phone]
    [StringLength(20)]
    public string Telefono { get; set; } = default!;

    [Required]
    [StringLength(200)]
    public string Direccion { get; set; } = default!;
}