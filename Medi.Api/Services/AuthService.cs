using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Medi.Application.DTOs;
using Medi.Application.Interfaces;
using Medi.Domain.Entities;
using Medi.Infrastructure.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace Medi.Api.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IConfiguration _configuration;

    public AuthService(IUsuarioRepository usuarioRepository, IConfiguration configuration)
    {
        _usuarioRepository = usuarioRepository;
        _configuration = configuration;
    }

    public async Task<AuthResponseDto> RegisterAsync(RegisterUserDto dto)
    {
        if (await _usuarioRepository.GetByUsernameAsync(dto.Username.Trim()) != null)
            throw new InvalidOperationException("Ese username ya existe.");

        if (await _usuarioRepository.GetByEmailAsync(dto.Email.Trim().ToLower()) != null)
            throw new InvalidOperationException("Ese email ya existe.");

        var usuario = new Usuario
        {
            Nombre = dto.Nombre.Trim(),
            Username = dto.Username.Trim(),
            Email = dto.Email.Trim().ToLower(),
            Rol = dto.Rol.Trim(),
            PasswordHash = HashPassword(dto.Password),
            CreatedBy = dto.Username.Trim()
        };

        await _usuarioRepository.AddAsync(usuario);

        return BuildAuthResponse(usuario);
    }

    public async Task<AuthResponseDto> LoginAsync(LoginDto dto)
    {
        var input = dto.UsernameOrEmail.Trim();

        Usuario? usuario;
        if (input.Contains("@"))
            usuario = await _usuarioRepository.GetByEmailAsync(input.ToLower());
        else
            usuario = await _usuarioRepository.GetByUsernameAsync(input);

        if (usuario == null || !VerifyPassword(dto.Password, usuario.PasswordHash))
            throw new UnauthorizedAccessException("Credenciales inválidas.");

        return BuildAuthResponse(usuario);
    }

    private AuthResponseDto BuildAuthResponse(Usuario usuario)
    {
        var token = GenerateToken(usuario);

        return new AuthResponseDto
        {
            Token = token,
            Username = usuario.Username,
            Rol = usuario.Rol,
            Nombre = usuario.Nombre
        };
    }

    private string GenerateToken(Usuario usuario)
    {
        var jwt = _configuration.GetSection("Jwt");
        var key = jwt["Key"]!;
        var issuer = jwt["Issuer"]!;
        var audience = jwt["Audience"]!;
        var expireMinutes = int.Parse(jwt["ExpireMinutes"]!);

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
            new Claim(ClaimTypes.Name, usuario.Username),
            new Claim(ClaimTypes.Email, usuario.Email),
            new Claim(ClaimTypes.Role, usuario.Rol)
        };

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: audience,
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(expireMinutes),
            signingCredentials: credentials
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    private static string HashPassword(string password)
    {
        byte[] salt = RandomNumberGenerator.GetBytes(16);
        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
        byte[] hash = pbkdf2.GetBytes(32);

        return $"{Convert.ToBase64String(salt)}.{Convert.ToBase64String(hash)}";
    }

    private static bool VerifyPassword(string password, string storedHash)
    {
        var parts = storedHash.Split('.');
        if (parts.Length != 2) return false;

        byte[] salt = Convert.FromBase64String(parts[0]);
        byte[] expectedHash = Convert.FromBase64String(parts[1]);

        using var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 100000, HashAlgorithmName.SHA256);
        byte[] actualHash = pbkdf2.GetBytes(32);

        return CryptographicOperations.FixedTimeEquals(actualHash, expectedHash);
    }
}