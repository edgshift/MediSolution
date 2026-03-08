using Medi.Application.DTOs;
using Medi.Application.Interfaces;
using Medi.Domain.Entities;
using Medi.Infrastructure.Interfaces;

namespace Medi.Application.Services
{
    public class PacienteService : IPacienteService
    {
        private readonly IPacienteRepository _pacienteRepository;
        private readonly ICurrentUserService _currentUserService;

        public PacienteService(IPacienteRepository pacienteRepository, ICurrentUserService currentUserService)
        {
            _pacienteRepository = pacienteRepository;
            _currentUserService = currentUserService;
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
            if (pacienteDto.FechaNacimiento.Date >= DateTime.UtcNow.Date)
                throw new InvalidOperationException("La fecha de nacimiento debe ser anterior al día actual.");

            if (await _pacienteRepository.GetByCedulaAsync(pacienteDto.Cedula) != null)
                throw new InvalidOperationException("Ya existe un paciente con esa cédula.");

            if (await _pacienteRepository.GetByEmailAsync(pacienteDto.Email) != null)
                throw new InvalidOperationException("Ya existe un paciente con ese correo.");

            var paciente = new Paciente
            {
                Nombre = pacienteDto.Nombre.Trim(),
                Apellido = pacienteDto.Apellido.Trim(),
                Cedula = pacienteDto.Cedula.Trim(),
                Telefono = pacienteDto.Telefono.Trim(),
                Email = pacienteDto.Email.Trim().ToLower(),
                FechaNacimiento = pacienteDto.FechaNacimiento.Date,
                Direccion = pacienteDto.Direccion.Trim(),
                CreatedBy = _currentUserService.GetUsername()
            };

            var result = await _pacienteRepository.AddAsync(paciente);
            pacienteDto.Id = result.Id;
            return pacienteDto;
        }

        public async Task<bool> UpdateAsync(int id, PacienteDto pacienteDto)
        {
            if (id != pacienteDto.Id) return false;

            var paciente = await _pacienteRepository.GetByIdAsync(id);
            if (paciente == null) return false;

            var otroCedula = await _pacienteRepository.GetByCedulaAsync(pacienteDto.Cedula);
            if (otroCedula != null && otroCedula.Id != id)
                throw new InvalidOperationException("Ya existe otro paciente con esa cédula.");

            var otroEmail = await _pacienteRepository.GetByEmailAsync(pacienteDto.Email);
            if (otroEmail != null && otroEmail.Id != id)
                throw new InvalidOperationException("Ya existe otro paciente con ese correo.");

            paciente.Nombre = pacienteDto.Nombre.Trim();
            paciente.Apellido = pacienteDto.Apellido.Trim();
            paciente.Cedula = pacienteDto.Cedula.Trim();
            paciente.Telefono = pacienteDto.Telefono.Trim();
            paciente.Email = pacienteDto.Email.Trim().ToLower();
            paciente.FechaNacimiento = pacienteDto.FechaNacimiento.Date;
            paciente.Direccion = pacienteDto.Direccion.Trim();
            paciente.UpdatedAt = DateTime.UtcNow;
            paciente.UpdatedBy = _currentUserService.GetUsername();

            await _pacienteRepository.UpdateAsync(paciente);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _pacienteRepository.SoftDeleteAsync(id, _currentUserService.GetUsername());
        }
    }
}