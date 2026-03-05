using Medi.Domain.Entities;

namespace Medi.Infrastructure.Interfaces;

public interface IPacienteRepository
{
    Task<Paciente> AddAsync(Paciente paciente);
    Task<Paciente?> GetByIdAsync(int id);
    Task<List<Paciente>> GetAllAsync();
    Task UpdateAsync(Paciente paciente);
    Task SoftDeleteAsync(int id);
}