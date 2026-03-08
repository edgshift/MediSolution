using Medi.Domain.Entities;

namespace Medi.Infrastructure.Interfaces;

public interface IPacienteRepository
{
    Task<Paciente> AddAsync(Paciente paciente);
    Task<Paciente?> GetByIdAsync(int id);
    Task<List<Paciente>> GetAllAsync();
    Task<bool> ExistsAsync(int id);
    Task<Paciente?> GetByCedulaAsync(string cedula);
    Task<Paciente?> GetByEmailAsync(string email);
    Task UpdateAsync(Paciente paciente);
    Task<bool> SoftDeleteAsync(int id, string? updatedBy);
}