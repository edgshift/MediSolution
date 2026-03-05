using Medi.Domain.Core;

namespace Medi.Domain.Entities;

public class Tratamiento : BaseEntity
{
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public decimal Costo { get; set; }
}