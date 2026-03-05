using Medi.Domain.Entities;
using Medi.Infrastructure.Context;
using Medi.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Medi.Infrastructure.Repositories
{
    public class SesionRepository : ISesionRepository
    {
        private readonly MediContext _context;

        public SesionRepository(MediContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sesion>> GetAllAsync()
        {
            return await _context.Sesiones
                .Include(s => s.Paciente)
                .Include(s => s.Doctor)
                .Include(s => s.Tratamiento)
                .Where(s => !s.IsDeleted)
                .ToListAsync();
        }

        public async Task<Sesion?> GetByIdAsync(int id)
        {
            return await _context.Sesiones
                .Include(s => s.Paciente)
                .Include(s => s.Doctor)
                .Include(s => s.Tratamiento)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
        }

        public async Task<Sesion> AddAsync(Sesion entity)
        {
            _context.Sesiones.Add(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        public async Task<bool> UpdateAsync(Sesion entity)
        {
            entity.UpdatedAt = DateTime.UtcNow;
            _context.Sesiones.Update(entity);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> SoftDeleteAsync(int id)
        {
            var entity = await _context.Sesiones.FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);
            if (entity == null) return false;

            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
            return await _context.SaveChangesAsync() > 0;
        }
    }
}