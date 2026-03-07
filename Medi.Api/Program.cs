using Medi.Application.Interfaces;
using Medi.Application.Services;
using Medi.Infrastructure.Context;
using Medi.Infrastructure.Interfaces;
using Medi.Infrastructure.Repositories;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<MediContext>(options =>
    options.UseSqlite(
        builder.Configuration.GetConnectionString("Default"),
        b => b.MigrationsAssembly("Medi.Infrastructure")
    )
);

builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
builder.Services.AddScoped<IDoctorRepository, DoctorRepository>();
builder.Services.AddScoped<ITratamientoRepository, TratamientoRepository>();
builder.Services.AddScoped<ISesionRepository, SesionRepository>();

builder.Services.AddScoped<IPacienteService, PacienteService>();
builder.Services.AddScoped<IDoctorService, DoctorService>();
builder.Services.AddScoped<ITratamientoService, TratamientoService>();
builder.Services.AddScoped<ISesionService, SesionService>();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapControllers();
app.Run();