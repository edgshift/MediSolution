namespace Medi.Application.Interfaces;

public interface ICurrentUserService
{
    string? GetUsername();
    string? GetRole();
}