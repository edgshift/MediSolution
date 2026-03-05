using Microsoft.EntityFrameworkCore;
using Medi.Domain.Core;
using Medi.Infrastructure.Context;

namespace Medi.Infrastructure.Core;

public abstract class BaseRepository<T> where T : BaseEntity
{
    protected readonly MediContext _db;
    protected readonly DbSet<T> _set;

    protected BaseRepository(MediContext db)
    {
        _db = db;
        _set = db.Set<T>();
    }

    public async Task<T> AddAsync(T entity)
    {
        await _set.AddAsync(entity);
        await _db.SaveChangesAsync();
        return entity;
    }

    public Task<T?> GetByIdAsync(int id)
        => _set.AsNoTracking()
               .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

    public Task<List<T>> GetAllAsync()
        => _set.AsNoTracking()
               .Where(x => !x.IsDeleted)
               .ToListAsync();

    public async Task UpdateAsync(T entity)
    {
        entity.UpdatedAt = DateTime.UtcNow;
        _set.Update(entity);
        await _db.SaveChangesAsync();
    }

    public async Task SoftDeleteAsync(int id)
    {
        var entity = await _set.FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        if (entity is null) return;

        entity.IsDeleted = true;
        entity.UpdatedAt = DateTime.UtcNow;
        await _db.SaveChangesAsync();
    }
}