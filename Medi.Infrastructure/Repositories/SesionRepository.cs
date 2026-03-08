using Medi.Domain.Entities;
using Medi.Infrastructure.Context;
using Medi.Infrastructure.Core;
using Medi.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Medi.Infrastructure.Repositories
{
    public class SesionRepository : BaseRepository<Sesion>, ISesionRepository
    {
        public SesionRepository(MediContext db) : base(db)
        {
        }

        public new Task<Sesion?> GetByIdAsync(int id)
            => _db.Sesiones
                .AsNoTracking()
                .Include(s => s.Paciente)
                .Include(s => s.Doctor)
                .Include(s => s.Tratamiento)
                .FirstOrDefaultAsync(s => s.Id == id && !s.IsDeleted);

        public new Task<List<Sesion>> GetAllAsync()
            => _db.Sesiones
                .AsNoTracking()
                .Include(s => s.Paciente)
                .Include(s => s.Doctor)
                .Include(s => s.Tratamiento)
                .Where(s => !s.IsDeleted)
                .OrderBy(s => s.Id)
                .ToListAsync();
    }
}