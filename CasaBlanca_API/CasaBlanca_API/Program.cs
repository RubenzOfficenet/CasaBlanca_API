
using CasaBlanca_API.EndPoints;
using CasaBlanca_API.Implementations;
using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Mappings;
using CasaBlanca_API.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AUTOMAPPER
builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());

// CORS: allow the SPA running on https://localhost:7128 to call this API
// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy",
        policy =>
        {
            policy.WithOrigins("http://localhost:4200")
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        });
});

// Interfaces and Implementations
builder.Services.AddScoped<IInmuebleService, InmuebleService>();
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<ICatalogosService, CatalogosService>();
builder.Services.AddScoped<IIngresoService, IngresoService>();
builder.Services.AddScoped<IEgresoService, EgresosService>();



// Register EF DbContext for Usuarios endpoints
builder.Services.AddDbContext<CasaBlancaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection")));


var app = builder.Build();
// Debe ir antes de MapControllers()
app.UseCors("AngularPolicy");



// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();

}

app.UseHttpsRedirection();

app.ConfigureInmueblesEndpoints();
app.ConfigureUsuarioEndpoints();
app.ConfigureCatalogosEndpoints();
app.ConfigureIngresosEndPoints();
app.ConfigureEgresosEndPoints();

app.Run();
