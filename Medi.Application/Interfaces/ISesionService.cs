using Medi.Application.DTOs;

namespace Medi.Application.Interfaces
{
    public interface ISesionService
    {
        Task<IEnumerable<SesionDto>> GetAllAsync();
        Task<SesionDto?> GetByIdAsync(int id);
        Task<SesionDto> CreateAsync(SesionDto sesionDto);
        Task UpdateAsync(SesionDto sesionDto);
        Task DeleteAsync(int id);
    }
}
