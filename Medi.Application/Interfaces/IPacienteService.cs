using Medi.Application.DTOs;

namespace Medi.Application.Interfaces
{
    public interface IPacienteService
    {
        Task<IEnumerable<PacienteDto>> GetAllAsync();
        Task<PacienteDto?> GetByIdAsync(int id);
        Task<PacienteDto> CreateAsync(PacienteDto pacienteDto);
        Task UpdateAsync(PacienteDto pacienteDto);
        Task DeleteAsync(int id);
    }
}
