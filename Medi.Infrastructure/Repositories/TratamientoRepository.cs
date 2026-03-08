using Medi.Domain.Entities;
using Medi.Infrastructure.Context;
using Medi.Infrastructure.Core;
using Medi.Infrastructure.Interfaces;

namespace Medi.Infrastructure.Repositories
{
    public class TratamientoRepository : BaseRepository<Tratamiento>, ITratamientoRepository
    {
        public TratamientoRepository(MediContext context) : base(context)
        {
        }
    }
}