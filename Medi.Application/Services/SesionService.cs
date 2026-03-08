using Medi.Application.DTOs;
using Medi.Application.Interfaces;
using Medi.Domain.Entities;
using Medi.Infrastructure.Interfaces;

namespace Medi.Application.Services
{
    public class SesionService : ISesionService
    {
        private readonly ISesionRepository _sesionRepository;
        private readonly IPacienteRepository _pacienteRepository;
        private readonly IDoctorRepository _doctorRepository;
        private readonly ITratamientoRepository _tratamientoRepository;
        private readonly ICurrentUserService _currentUserService;

        public SesionService(
            ISesionRepository sesionRepository,
            IPacienteRepository pacienteRepository,
            IDoctorRepository doctorRepository,
            ITratamientoRepository tratamientoRepository,
            ICurrentUserService currentUserService)
        {
            _sesionRepository = sesionRepository;
            _pacienteRepository = pacienteRepository;
            _doctorRepository = doctorRepository;
            _tratamientoRepository = tratamientoRepository;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<SesionDto>> GetAllAsync()
        {
            var sesiones = await _sesionRepository.GetAllAsync();

            return sesiones.Select(s => new SesionDto
            {
                Id = s.Id,
                FechaHora = s.FechaHora,
                DuracionMinutos = s.DuracionMinutos,
                Notas = s.Notas,
                PacienteId = s.PacienteId,
                DoctorId = s.DoctorId,
                TratamientoId = s.TratamientoId
            });
        }

        public async Task<SesionDto?> GetByIdAsync(int id)
        {
            var sesion = await _sesionRepository.GetByIdAsync(id);
            if (sesion == null) return null;

            return new SesionDto
            {
                Id = sesion.Id,
                FechaHora = sesion.FechaHora,
                DuracionMinutos = sesion.DuracionMinutos,
                Notas = sesion.Notas,
                PacienteId = sesion.PacienteId,
                DoctorId = sesion.DoctorId,
                TratamientoId = sesion.TratamientoId
            };
        }

        public async Task<SesionDto> CreateAsync(SesionDto sesionDto)
        {
            if (!await _pacienteRepository.ExistsAsync(sesionDto.PacienteId))
                throw new InvalidOperationException("El paciente no existe.");

            if (!await _doctorRepository.ExistsAsync(sesionDto.DoctorId))
                throw new InvalidOperationException("El doctor no existe.");

            if (!await _tratamientoRepository.ExistsAsync(sesionDto.TratamientoId))
                throw new InvalidOperationException("El tratamiento no existe.");

            var sesion = new Sesion
            {
                FechaHora = sesionDto.FechaHora,
                DuracionMinutos = sesionDto.DuracionMinutos,
                Notas = sesionDto.Notas,
                PacienteId = sesionDto.PacienteId,
                DoctorId = sesionDto.DoctorId,
                TratamientoId = sesionDto.TratamientoId,
                CreatedBy = _currentUserService.GetUsername()
            };

            var created = await _sesionRepository.AddAsync(sesion);

            return new SesionDto
            {
                Id = created.Id,
                FechaHora = created.FechaHora,
                DuracionMinutos = created.DuracionMinutos,
                Notas = created.Notas,
                PacienteId = created.PacienteId,
                DoctorId = created.DoctorId,
                TratamientoId = created.TratamientoId
            };
        }

        public async Task<bool> UpdateAsync(int id, SesionDto sesionDto)
        {
            if (id != sesionDto.Id) return false;

            var sesion = await _sesionRepository.GetByIdAsync(id);
            if (sesion == null) return false;

            if (!await _pacienteRepository.ExistsAsync(sesionDto.PacienteId))
                throw new InvalidOperationException("El paciente no existe.");

            if (!await _doctorRepository.ExistsAsync(sesionDto.DoctorId))
                throw new InvalidOperationException("El doctor no existe.");

            if (!await _tratamientoRepository.ExistsAsync(sesionDto.TratamientoId))
                throw new InvalidOperationException("El tratamiento no existe.");

            sesion.FechaHora = sesionDto.FechaHora;
            sesion.DuracionMinutos = sesionDto.DuracionMinutos;
            sesion.Notas = sesionDto.Notas;
            sesion.PacienteId = sesionDto.PacienteId;
            sesion.DoctorId = sesionDto.DoctorId;
            sesion.TratamientoId = sesionDto.TratamientoId;
            sesion.UpdatedAt = DateTime.UtcNow;
            sesion.UpdatedBy = _currentUserService.GetUsername();

            await _sesionRepository.UpdateAsync(sesion);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _sesionRepository.SoftDeleteAsync(id, _currentUserService.GetUsername());
        }
    }
}