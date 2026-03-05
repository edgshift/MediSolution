using Medi.Domain.Entities;
using Medi.Infrastructure.Context;
using Medi.Infrastructure.Core;
using Medi.Infrastructure.Interfaces;

namespace Medi.Infrastructure.Repositories;

public class PacienteRepository : BaseRepository<Paciente>, IPacienteRepository
{
    public PacienteRepository(MediContext db) : base(db) { }
}