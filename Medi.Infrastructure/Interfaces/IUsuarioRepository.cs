using Medi.Domain.Entities;

namespace Medi.Infrastructure.Interfaces;

public interface IUsuarioRepository
{
    Task<Usuario> AddAsync(Usuario usuario);
    Task<Usuario?> GetByUsernameAsync(string username);
    Task<Usuario?> GetByEmailAsync(string email);
}