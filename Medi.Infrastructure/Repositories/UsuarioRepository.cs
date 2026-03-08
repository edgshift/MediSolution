using Medi.Domain.Entities;
using Medi.Infrastructure.Context;
using Medi.Infrastructure.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Medi.Infrastructure.Repositories;

public class UsuarioRepository : IUsuarioRepository
{
    private readonly MediContext _context;

    public UsuarioRepository(MediContext context)
    {
        _context = context;
    }

    public async Task<Usuario> AddAsync(Usuario usuario)
    {
        _context.Usuarios.Add(usuario);
        await _context.SaveChangesAsync();
        return usuario;
    }

    public Task<Usuario?> GetByUsernameAsync(string username)
        => _context.Usuarios.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Username == username && !x.IsDeleted);

    public Task<Usuario?> GetByEmailAsync(string email)
        => _context.Usuarios.AsNoTracking()
            .FirstOrDefaultAsync(x => x.Email == email && !x.IsDeleted);
}