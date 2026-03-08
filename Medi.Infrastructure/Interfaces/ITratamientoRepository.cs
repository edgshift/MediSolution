using Medi.Domain.Entities;

namespace Medi.Infrastructure.Interfaces;

public interface ITratamientoRepository
{
    Task<Tratamiento> AddAsync(Tratamiento tratamiento);
    Task<Tratamiento?> GetByIdAsync(int id);
    Task<List<Tratamiento>> GetAllAsync();
    Task<bool> ExistsAsync(int id);
    Task UpdateAsync(Tratamiento tratamiento);
    Task<bool> SoftDeleteAsync(int id, string? updatedBy);
}