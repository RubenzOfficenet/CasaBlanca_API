
using CasaBlanca_API.EndPoints;
using CasaBlanca_API.Implementations;
using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models;
using CasaBlanca_API.Models.DTO.Casa;
using Core.Mappy;
using Core.Mappy.Extensions;
using Core.Mappy.Interfaces;
using Microsoft.EntityFrameworkCore;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
// builder.Services.AddOpenApi();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// AUTOMAPPER
//builder.Services.AddAutoMapper(cfg => { }, AppDomain.CurrentDomain.GetAssemblies());


// CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AngularPolicy",
        policy =>
        {
            policy.WithOrigins(
                    "http://localhost:61160",
                    "http://localhost:8080")
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
builder.Services.AddScoped<IEventoService, EventoService>();
builder.Services.AddScoped<IEgresoEventoService, EgresoEventoService>();
builder.Services.AddScoped<IEgresoAnaliticoService, EgresoAnaliticoService>();

builder.Services.AddTransient<IMapper, Mapper>();


// Register EF DbContext for Usuarios endpoints
builder.Services.AddDbContext<CasaBlancaDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefultConnection")));


var app = builder.Build();

var mapper = app.Services.GetRequiredService<IMapper>();

// mapper DTOs
mapper.RegisterMappings(typeof(CasaMappingProfile).Assembly);


// Debe ir antes de MapControllers()
app.UseCors("AngularPolicy");




// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();

    app.UseSwagger();
    app.UseSwaggerUI();

}

//await app.ApplyMigration(builder.Environment);
//app.MapControllers();

app.UseHttpsRedirection();

app.ConfigureInmueblesEndpoints();
app.ConfigureUsuarioEndpoints();
app.ConfigureCatalogosEndpoints();
app.ConfigureIngresosEndPoints();
app.ConfigureEgresosEndPoints();
app.ConfigureEventosIngresosEndPoint();
app.ConfigureEventosEgresosEndPoint();
app.ConfigureResumenAnaliticoEndPoint();
app.ConfigurePasswordEndPoint();

app.Run();
