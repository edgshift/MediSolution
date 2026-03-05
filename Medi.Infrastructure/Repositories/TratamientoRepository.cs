using Medi.Domain.Entities;
using Medi.Infrastructure.Context;
using Medi.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Medi.Infrastructure.Repositories
{
    public class TratamientoRepository : ITratamientoRepository
    {
        private readonly MediContext _context;

        public TratamientoRepository(MediContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tratamiento>> GetAllAsync()
            => await _context.Tratamientos
                .Where(t => !t.IsDeleted)
                .ToListAsync();

        public async Task<Tratamiento?> GetByIdAsync(int id)
            => await _context.Tratamientos
                .FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);

        public async Task<Tratamiento> AddAsync(Tratamiento tratamiento)
        {
            _context.Tratamientos.Add(tratamiento);
            await _context.SaveChangesAsync();
            return tratamiento;
        }

        public async Task UpdateAsync(Tratamiento tratamiento)
        {
            tratamiento.UpdatedAt = DateTime.UtcNow;
            _context.Tratamientos.Update(tratamiento);
            await _context.SaveChangesAsync();
        }

        public async Task SoftDeleteAsync(int id)
        {
            var entity = await _context.Tratamientos.FirstOrDefaultAsync(t => t.Id == id && !t.IsDeleted);
            if (entity == null) return;

            entity.IsDeleted = true;
            entity.UpdatedAt = DateTime.UtcNow;
            await _context.SaveChangesAsync();
        }
    }
}