using Medi.Domain.Entities;

namespace Medi.Infrastructure.Interfaces;

public interface ISesionRepository
{
    Task<Sesion> AddAsync(Sesion entity);
    Task<Sesion?> GetByIdAsync(int id);
    Task<List<Sesion>> GetAllAsync();
    Task<bool> ExistsAsync(int id);
    Task UpdateAsync(Sesion entity);
    Task<bool> SoftDeleteAsync(int id, string? updatedBy);
}