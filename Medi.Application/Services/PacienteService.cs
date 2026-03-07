using Medi.Application.DTOs;
using Medi.Application.Interfaces;
using Medi.Domain.Entities;
using Medi.Infrastructure.Interfaces;

namespace Medi.Application.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _pacienteRepository;

        public PacienteService(IPacienteRepository pacienteRepository)
        {
            _pacienteRepository = pacienteRepository;
        }

        public async Task<IEnumerable<PacienteDto>> GetAllAsync()
        {
            var pacientes = await _pacienteRepository.GetAllAsync();
            return pacientes.Select(p => new PacienteDto
            {
                Id = p.Id,
                Nombre = p.Nombre,
                Apellido = p.Apellido,
                Cedula = p.Cedula,
                Telefono = p.Telefono,
                Email = p.Email,
                FechaNacimiento = p.FechaNacimiento,
                Direccion = p.Direccion
            });
        }

        public async Task<PacienteDto?> GetByIdAsync(int id)
        {
            var paciente = await _pacienteRepository.GetByIdAsync(id);
            if (paciente == null) return null;

            return new PacienteDto
            {
                Id = paciente.Id,
                Nombre = paciente.Nombre,
                Apellido = paciente.Apellido,
                Cedula = paciente.Cedula,
                Telefono = paciente.Telefono,
                Email = paciente.Email,
                FechaNacimiento = paciente.FechaNacimiento,
                Direccion = paciente.Direccion
            };
        }

        public async Task<PacienteDto> CreateAsync(PacienteDto pacienteDto)
        {
            var paciente = new Paciente
            {
                Nombre = pacienteDto.Nombre,
                Apellido = pacienteDto.Apellido,
                Cedula = pacienteDto.Cedula,
                Telefono = pacienteDto.Telefono,
                Email = pacienteDto.Email,
                FechaNacimiento = pacienteDto.FechaNacimiento,
                Direccion = pacienteDto.Direccion
            };

            var result = await _pacienteRepository.AddAsync(paciente);
            pacienteDto.Id = result.Id;
            return pacienteDto;
        }

        public async Task UpdateAsync(PacienteDto pacienteDto)
        {
            var paciente = await _pacienteRepository.GetByIdAsync(pacienteDto.Id);
            if (paciente != null)
            {
                paciente.Nombre = pacienteDto.Nombre;
                paciente.Apellido = pacienteDto.Apellido;
                paciente.Cedula = pacienteDto.Cedula;
                paciente.Telefono = pacienteDto.Telefono;
                paciente.Email = pacienteDto.Email;
                paciente.FechaNacimiento = pacienteDto.FechaNacimiento;
                paciente.Direccion = pacienteDto.Direccion;
                paciente.UpdatedAt = DateTime.UtcNow;

                await _pacienteRepository.UpdateAsync(paciente);
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _pacienteRepository.SoftDeleteAsync(id);
        }
    }
}
