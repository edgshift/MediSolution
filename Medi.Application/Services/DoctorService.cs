using Medi.Application.DTOs;
using Medi.Application.Interfaces;
using Medi.Domain.Entities;
using Medi.Infrastructure.Interfaces;

namespace Medi.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;

        public DoctorService(IDoctorRepository doctorRepository)
        {
            _doctorRepository = doctorRepository;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllAsync()
        {
            var doctores = await _doctorRepository.GetAllAsync();
            return doctores.Select(d => new DoctorDto
            {
                Id = d.Id,
                Nombre = d.Nombre,
                Apellido = d.Apellido,
                Especialidad = d.Especialidad,
                Telefono = d.Telefono
            });
        }

        public async Task<DoctorDto?> GetByIdAsync(int id)
        {
            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null) return null;

            return new DoctorDto
            {
                Id = doctor.Id,
                Nombre = doctor.Nombre,
                Apellido = doctor.Apellido,
                Especialidad = doctor.Especialidad,
                Telefono = doctor.Telefono
            };
        }

        public async Task<DoctorDto> CreateAsync(DoctorDto doctorDto)
        {
            var doctor = new Doctor
            {
                Nombre = doctorDto.Nombre,
                Apellido = doctorDto.Apellido,
                Especialidad = doctorDto.Especialidad,
                Telefono = doctorDto.Telefono
            };

            var result = await _doctorRepository.AddAsync(doctor);
            doctorDto.Id = result.Id;
            return doctorDto;
        }

        public async Task UpdateAsync(DoctorDto doctorDto)
        {
            var doctor = await _doctorRepository.GetByIdAsync(doctorDto.Id);
            if (doctor != null)
            {
                doctor.Nombre = doctorDto.Nombre;
                doctor.Apellido = doctorDto.Apellido;
                doctor.Especialidad = doctorDto.Especialidad;
                doctor.Telefono = doctorDto.Telefono;
                doctor.UpdatedAt = DateTime.UtcNow;

                await _doctorRepository.UpdateAsync(doctor);
            }
        }

        public async Task DeleteAsync(int id)
        {
            await _doctorRepository.SoftDeleteAsync(id);
        }
    }
}
