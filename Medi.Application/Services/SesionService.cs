using Medi.Application.DTOs;
using Medi.Application.Interfaces;
using Medi.Domain.Entities;
using Medi.Infrastructure.Interfaces;

namespace Medi.Application.Services
{
    public class SesionService : ISesionService
    {
        private readonly ISesionRepository _sesionRepository;

        public SesionService(ISesionRepository sesionRepository)
        {
            _sesionRepository = sesionRepository;
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
            var sesion = new Sesion
            {
                FechaHora = sesionDto.FechaHora,
                DuracionMinutos = sesionDto.DuracionMinutos,
                Notas = sesionDto.Notas,
                PacienteId = sesionDto.PacienteId,
                DoctorId = sesionDto.DoctorId,
                TratamientoId = sesionDto.TratamientoId
            };

            var result = await _sesionRepository.AddAsync(sesion);
            sesionDto.Id = result.Id;
            return sesionDto;
        }

        public async Task UpdateAsync(SesionDto sesionDto)
        {
            var sesion = await _sesionRepository.GetByIdAsync(sesionDto.Id);
            if (sesion != null)
            {
                sesion.FechaHora = sesionDto.FechaHora;
                sesion.DuracionMinutos = sesionDto.DuracionMinutos;
                sesion.Notas = sesionDto.Notas;
                sesion.PacienteId = sesionDto.PacienteId;
                sesion.DoctorId = sesionDto.DoctorId;
                sesion.TratamientoId = sesionDto.TratamientoId;
                sesion.UpdatedAt = DateTime.UtcNow;

                await _sesionRepository.UpdateAsync(sesion);
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _sesionRepository.SoftDeleteAsync(id);
        }
    }
}
