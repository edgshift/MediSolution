using Medi.Application.DTOs;

namespace Medi.Application.Interfaces
{
    public interface ITratamientoService
    {
        Task<IEnumerable<TratamientoDto>> GetAllAsync();
        Task<TratamientoDto?> GetByIdAsync(int id);
        Task<TratamientoDto> CreateAsync(TratamientoDto tratamientoDto);
        Task UpdateAsync(TratamientoDto tratamientoDto);
        Task DeleteAsync(int id);
    }
}
