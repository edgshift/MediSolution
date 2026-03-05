using Microsoft.EntityFrameworkCore;
using Medi.Domain.Entities;

namespace Medi.Infrastructure.Context;

public class MediContext : DbContext
{
    public MediContext(DbContextOptions<MediContext> options) : base(options) { }

    public DbSet<Paciente> Pacientes => Set<Paciente>();
    public DbSet<Doctor> Doctores => Set<Doctor>();
    public DbSet<Tratamiento> Tratamientos => Set<Tratamiento>();
    public DbSet<Sesion> Sesiones => Set<Sesion>();
}