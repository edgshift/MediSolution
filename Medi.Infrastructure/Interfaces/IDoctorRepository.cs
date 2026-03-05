using System.Collections.Generic;
using System.Threading.Tasks;
using Medi.Domain.Entities;

namespace Medi.Infrastructure.Interfaces
{
    public interface IDoctorRepository
    {
        Task<Doctor> AddAsync(Doctor doctor);
        Task<Doctor?> GetByIdAsync(int id);
        Task<List<Doctor>> GetAllAsync();
        Task UpdateAsync(Doctor doctor);
        Task SoftDeleteAsync(int id);
    }
}