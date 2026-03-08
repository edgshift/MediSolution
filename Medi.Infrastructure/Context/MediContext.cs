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
    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Paciente>(entity =>
        {
            entity.ToTable("Pacientes");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Apellido).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Cedula).IsRequired().HasMaxLength(20);
            entity.Property(x => x.Telefono).IsRequired().HasMaxLength(20);
            entity.Property(x => x.Email).IsRequired().HasMaxLength(150);
            entity.Property(x => x.Direccion).IsRequired().HasMaxLength(200);
            entity.Property(x => x.CreatedBy).HasMaxLength(50);
            entity.Property(x => x.UpdatedBy).HasMaxLength(50);

            entity.HasIndex(x => x.Cedula).IsUnique();
            entity.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<Doctor>(entity =>
        {
            entity.ToTable("Doctores");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Apellido).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Cedula).IsRequired().HasMaxLength(20);
            entity.Property(x => x.Sexo).IsRequired().HasMaxLength(20);
            entity.Property(x => x.Especialidad).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Telefono).IsRequired().HasMaxLength(20);
            entity.Property(x => x.Direccion).IsRequired().HasMaxLength(200);
            entity.Property(x => x.CreatedBy).HasMaxLength(50);
            entity.Property(x => x.UpdatedBy).HasMaxLength(50);

            entity.HasIndex(x => x.Cedula).IsUnique();
        });

        modelBuilder.Entity<Tratamiento>(entity =>
        {
            entity.ToTable("Tratamientos");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Descripcion).IsRequired().HasMaxLength(500);
            entity.Property(x => x.Costo).HasColumnType("decimal(18,2)");
            entity.Property(x => x.CreatedBy).HasMaxLength(50);
            entity.Property(x => x.UpdatedBy).HasMaxLength(50);
        });

        modelBuilder.Entity<Sesion>(entity =>
        {
            entity.ToTable("Sesiones");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Notas).HasMaxLength(500);
            entity.Property(x => x.CreatedBy).HasMaxLength(50);
            entity.Property(x => x.UpdatedBy).HasMaxLength(50);

            entity.HasOne(x => x.Paciente)
                .WithMany(x => x.Sesiones)
                .HasForeignKey(x => x.PacienteId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Doctor)
                .WithMany(x => x.Sesiones)
                .HasForeignKey(x => x.DoctorId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(x => x.Tratamiento)
                .WithMany()
                .HasForeignKey(x => x.TratamientoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.HasKey(x => x.Id);

            entity.Property(x => x.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(x => x.Username).IsRequired().HasMaxLength(50);
            entity.Property(x => x.Email).IsRequired().HasMaxLength(150);
            entity.Property(x => x.PasswordHash).IsRequired();
            entity.Property(x => x.Rol).IsRequired().HasMaxLength(30);

            entity.HasIndex(x => x.Username).IsUnique();
            entity.HasIndex(x => x.Email).IsUnique();
        });
    }
}