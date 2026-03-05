using Medi.Domain.Entities;
using Medi.Infrastructure.Context;
using Medi.Infrastructure.Core;
using Medi.Infrastructure.Interfaces;

namespace Medi.Infrastructure.Repositories
{
    public class DoctorRepository
        : BaseRepository<Doctor>, IDoctorRepository
    {
        public DoctorRepository(MediContext db) : base(db) { }
    }
}