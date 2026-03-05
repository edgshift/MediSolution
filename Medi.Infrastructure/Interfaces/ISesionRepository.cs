using Medi.Domain.Entities;

namespace Medi.Infrastructure.Interfaces
{
    public interface ISesionRepository
    {
        Task<IEnumerable<Sesion>> GetAllAsync();
        Task<Sesion?> GetByIdAsync(int id);
        Task<Sesion> AddAsync(Sesion entity);
        Task<bool> UpdateAsync(Sesion entity);
        Task<bool> SoftDeleteAsync(int id);
    }
}