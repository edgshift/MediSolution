using Medi.Application.DTOs;

namespace Medi.Application.Interfaces
{
    public interface IPacienteService
    {
        Task<IEnumerable<PacienteDto>> GetAllAsync();
        Task<PacienteDto?> GetByIdAsync(int id);
        Task<PacienteDto> CreateAsync(PacienteDto pacienteDto);
        Task<bool> UpdateAsync(int id, PacienteDto pacienteDto);
        Task<bool> DeleteAsync(int id);
    }
}