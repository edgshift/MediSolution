using Medi.Application.DTOs;
using Medi.Application.Interfaces;
using Medi.Domain.Entities;
using Medi.Infrastructure.Interfaces;

namespace Medi.Application.Services
{
    public class DoctorService : IDoctorService
    {
        private readonly IDoctorRepository _doctorRepository;
        private readonly ICurrentUserService _currentUserService;

        public DoctorService(IDoctorRepository doctorRepository, ICurrentUserService currentUserService)
        {
            _doctorRepository = doctorRepository;
            _currentUserService = currentUserService;
        }

        public async Task<IEnumerable<DoctorDto>> GetAllAsync()
        {
            var doctores = await _doctorRepository.GetAllAsync();
            return doctores.Select(d => new DoctorDto
            {
                Id = d.Id,
                Nombre = d.Nombre,
                Apellido = d.Apellido,
                Cedula = d.Cedula,
                Sexo = d.Sexo,
                Especialidad = d.Especialidad,
                Telefono = d.Telefono,
                Direccion = d.Direccion
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
                Cedula = doctor.Cedula,
                Sexo = doctor.Sexo,
                Especialidad = doctor.Especialidad,
                Telefono = doctor.Telefono,
                Direccion = doctor.Direccion
            };
        }

        public async Task<DoctorDto> CreateAsync(DoctorDto doctorDto)
        {
            var existente = await _doctorRepository.GetByCedulaAsync(doctorDto.Cedula);
            if (existente != null)
                throw new InvalidOperationException("Ya existe un doctor con esa cédula.");

            var doctor = new Doctor
            {
                Nombre = doctorDto.Nombre.Trim(),
                Apellido = doctorDto.Apellido.Trim(),
                Cedula = doctorDto.Cedula.Trim(),
                Sexo = doctorDto.Sexo.Trim(),
                Especialidad = doctorDto.Especialidad.Trim(),
                Telefono = doctorDto.Telefono.Trim(),
                Direccion = doctorDto.Direccion.Trim(),
                CreatedBy = _currentUserService.GetUsername()
            };

            var result = await _doctorRepository.AddAsync(doctor);

            doctorDto.Id = result.Id;
            return doctorDto;
        }

        public async Task<bool> UpdateAsync(int id, DoctorDto doctorDto)
        {
            if (id != doctorDto.Id) return false;

            var doctor = await _doctorRepository.GetByIdAsync(id);
            if (doctor == null) return false;

            var otro = await _doctorRepository.GetByCedulaAsync(doctorDto.Cedula);
            if (otro != null && otro.Id != id)
                throw new InvalidOperationException("Ya existe otro doctor con esa cédula.");

            doctor.Nombre = doctorDto.Nombre.Trim();
            doctor.Apellido = doctorDto.Apellido.Trim();
            doctor.Cedula = doctorDto.Cedula.Trim();
            doctor.Sexo = doctorDto.Sexo.Trim();
            doctor.Especialidad = doctorDto.Especialidad.Trim();
            doctor.Telefono = doctorDto.Telefono.Trim();
            doctor.Direccion = doctorDto.Direccion.Trim();
            doctor.UpdatedAt = DateTime.UtcNow;
            doctor.UpdatedBy = _currentUserService.GetUsername();

            await _doctorRepository.UpdateAsync(doctor);
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _doctorRepository.SoftDeleteAsync(id, _currentUserService.GetUsername());
        }
    }
}