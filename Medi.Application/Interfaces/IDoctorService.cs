using Medi.Application.DTOs;

namespace Medi.Application.Interfaces
{
    public interface IDoctorService
    {
        Task<IEnumerable<DoctorDto>> GetAllAsync();
        Task<DoctorDto?> GetByIdAsync(int id);
        Task<DoctorDto> CreateAsync(DoctorDto doctorDto);
        Task UpdateAsync(DoctorDto doctorDto);
        Task DeleteAsync(int id);
    }
}
