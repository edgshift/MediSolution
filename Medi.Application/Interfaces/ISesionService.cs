using Medi.Application.DTOs;

namespace Medi.Application.Interfaces
{
    public interface ISesionService
    {
        Task<IEnumerable<SesionDto>> GetAllAsync();
        Task<SesionDto?> GetByIdAsync(int id);
        Task<SesionDto> CreateAsync(SesionDto sesionDto);
        Task<bool> UpdateAsync(int id, SesionDto sesionDto);
        Task<bool> DeleteAsync(int id);
    }
}