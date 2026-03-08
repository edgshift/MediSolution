using Medi.Application.DTOs;
using Medi.Application.Interfaces;
using Medi.Domain.Entities;
using Medi.Infrastructure.Interfaces;

namespace Medi.Application.Services
{
    public class TratamientoService : ITratamientoService
    {
        private readonly ITratamientoRepository _tratamientoRepository;
        private readonly ICurrentUserService _currentUserService;

        public TratamientoService(ITratamientoRepository tratamientoRepository, ICurrentUserService currentUserService)
        {
            _tratamientoRepository = tratamientoRepository;
            _currentUserService = currentUserService;
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
                Nombre = tratamientoDto.Nombre.Trim(),
                Descripcion = tratamientoDto.Descripcion.Trim(),
                Costo = tratamientoDto.Costo,
                CreatedBy = _currentUserService.GetUsername()
            };

            var created = await _tratamientoRepository.AddAsync(tratamiento);

            return new TratamientoDto
            {
                Id = created.Id,
                Nombre = created.Nombre,
                Descripcion = created.Descripcion,
                Costo = created.Costo
            };
        }

        public async Task<bool> UpdateAsync(int id, TratamientoDto tratamientoDto)
        {
            if (id != tratamientoDto.Id) return false;

            var tratamiento = await _tratamientoRepository.GetByIdAsync(id);
            if (tratamiento == null) return false;

            tratamiento.Nombre = tratamientoDto.Nombre.Trim();
            tratamiento.Descripcion = tratamientoDto.Descripcion.Trim();
            tratamiento.Costo = tratamientoDto.Costo;
            tratamiento.UpdatedAt = DateTime.UtcNow;
            tratamiento.UpdatedBy = _currentUserService.GetUsername();

            await _tratamientoRepository.UpdateAsync(tratamiento);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _tratamientoRepository.SoftDeleteAsync(id, _currentUserService.GetUsername());
        }
    }
}