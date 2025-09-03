using Microsoft.EntityFrameworkCore;
using apiTecnica.Models;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;

var builder = WebApplication.CreateBuilder(args);

// Configuración de la cadena de conexión MySQL
// TODO: Sacar cadena de conexión
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ??
    builder.Configuration["ConnectionStrings:DefaultConnection"] ??
    "server=localhost;port=3306;database=pruebaTecnica;user=root;password=root";

// Agregar soporte para Entity Framework Core con MySQL
builder.Services.AddDbContext<PruebaTecnicaContext>(options =>
    options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString)));

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
// Agregar soporte para controladores
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.WithOrigins("http://localhost:5173")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});


var app = builder.Build();



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseCors();
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Mapear los controladores
app.MapControllers();

// ...existing code...

app.Run();