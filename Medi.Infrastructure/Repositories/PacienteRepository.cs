using Medi.Domain.Entities;
using Medi.Infrastructure.Context;
using Medi.Infrastructure.Core;
using Medi.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Medi.Infrastructure.Repositories
{
    public class PacienteRepository : BaseRepository<Paciente>, IPacienteRepository
    {
        public PacienteRepository(MediContext db) : base(db) { }

        public Task<Paciente?> GetByCedulaAsync(string cedula)
            => _set.AsNoTracking().FirstOrDefaultAsync(x => x.Cedula == cedula && !x.IsDeleted);

        public Task<Paciente?> GetByEmailAsync(string email)
            => _set.AsNoTracking().FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted);
    }
}