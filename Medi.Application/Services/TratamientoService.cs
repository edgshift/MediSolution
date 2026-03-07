using Medi.Application.DTOs;
using Medi.Application.Interfaces;
using Medi.Domain.Entities;
using Medi.Infrastructure.Interfaces;

namespace Medi.Application.Services
{
    public class TratamientoService : ITratamientoService
    {
        private readonly ITratamientoRepository _tratamientoRepository;

        public TratamientoService(ITratamientoRepository tratamientoRepository)
        {
            _tratamientoRepository = tratamientoRepository;
        }

        public async Task<IEnumerable<TratamientoDto>> GetAllAsync()
        {
            var tratamientos = await _tratamientoRepository.GetAllAsync();
            return tratamientos.Select(t => new TratamientoDto
            {
                Id = t.Id,
                Nombre = t.Nombre,
                Descripcion = t.Descripcion,
                Costo = t.Costo
            });
        }

        public async Task<TratamientoDto?> GetByIdAsync(int id)
        {
            var tratamiento = await _tratamientoRepository.GetByIdAsync(id);
            if (tratamiento == null) return null;

            return new TratamientoDto
            {
                Id = tratamiento.Id,
                Nombre = tratamiento.Nombre,
                Descripcion = tratamiento.Descripcion,
                Costo = tratamiento.Costo
            };
        }

        public async Task<TratamientoDto> CreateAsync(TratamientoDto tratamientoDto)
        {
            var tratamiento = new Tratamiento
            {
                Nombre = tratamientoDto.Nombre,
                Descripcion = tratamientoDto.Descripcion,
                Costo = tratamientoDto.Costo
            };

            var result = await _tratamientoRepository.AddAsync(tratamiento);
            tratamientoDto.Id = result.Id;
            return tratamientoDto;
        }

        public async Task UpdateAsync(TratamientoDto tratamientoDto)
        {
            var tratamiento = await _tratamientoRepository.GetByIdAsync(tratamientoDto.Id);
            if (tratamiento != null)
            {
                tratamiento.Nombre = tratamientoDto.Nombre;
                tratamiento.Descripcion = tratamientoDto.Descripcion;
                tratamiento.Costo = tratamientoDto.Costo;
                tratamiento.UpdatedAt = DateTime.UtcNow;

                await _tratamientoRepository.UpdateAsync(tratamiento);
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _tratamientoRepository.SoftDeleteAsync(id);
        }
    }
}
