using Medi.Domain.Entities;
using Medi.Infrastructure.Context;
using Medi.Infrastructure.Core;
using Medi.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Medi.Infrastructure.Repositories
{
    public class DoctorRepository : BaseRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(MediContext db) : base(db) { }

        public Task<Doctor?> GetByCedulaAsync(string cedula)
            => _set.AsNoTracking().FirstOrDefaultAsync(x => x.Cedula == cedula && !x.IsDeleted);
    }
}