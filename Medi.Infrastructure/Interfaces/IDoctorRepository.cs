using Medi.Domain.Entities;

namespace Medi.Infrastructure.Interfaces;

public interface IDoctorRepository
{
    Task<Doctor> AddAsync(Doctor doctor);
    Task<Doctor?> GetByIdAsync(int id);
    Task<List<Doctor>> GetAllAsync();
    Task<bool> ExistsAsync(int id);
    Task<Doctor?> GetByCedulaAsync(string cedula);
    Task UpdateAsync(Doctor doctor);
    Task<bool> SoftDeleteAsync(int id, string? updatedBy);
}