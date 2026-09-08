using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models;
using CasaBlanca_API.Models.DTO;
using CasaBlanca_API.Models.DTO.Casa;
using CasaBlanca_API.Models.DTO.Casas;
using CasaBlanca_API.shared;
using Microsoft.AspNetCore.Http.HttpResults;
using System.Text.RegularExpressions;

namespace CasaBlanca_API.EndPoints;

public static class InmueblesEndpoints
{
    public static void ConfigureInmueblesEndpoints(this WebApplication app)
    {
        var connectionString = app.Configuration.GetConnectionString("DefultConnection");

        app.MapGet("/api/GetHouses", GetAllHouses).WithName("GetHouses").Produces<IEnumerable<CatalogoCasasDTO>>(200);
        app.MapGet("/api/GetHouseById/{id}", GetHouseById).WithName("GetHouseById").Produces<InmuebleReadDTO>(200).Produces(400).Produces(404).Produces(500);
        app.MapPost("/api/UpdateHouse", UpdateHouse).WithName("UpdateHouse").Accepts<UpdateCasaDTO>("application/json").Produces<int>(200).Produces<string>(400).Produces<string>(500);
        app.MapPost("/api/CreateHouse", CreateHuse).WithName("CreateHouse").Produces<string>(200).Produces<string>(400).Produces<string>(409).Produces<string>(500);
        app.MapGet("/api/GetEstadosOcupacion", GetEstadosOcupacion).WithName("GetEstadosOcupacion").Produces<string>(200).Produces<string>(500);
        app.MapGet("/api/GetHouseIdUbicacion", GetHouseIdUbicacion).WithName("GetHouseIdUbicacion").Produces<InmuebleReadDTO>(200).Produces(400).Produces(404).Produces(500);
    }

    public async static Task<IResult> UpdateHouse(UpdateCasaDTO inmueble, IInmuebleService inmuebleService)
    {
        try
        {
            if (inmueble == null || inmueble.Id <= 0)
                return Results.BadRequest("Invalid inmueble data. Id is required.");

            // Optional: validate NumeroCasa
            if (string.IsNullOrEmpty(inmueble.NumeroCasa))
                return Results.BadRequest("NumeroCasa is required.");

            int rows = await inmuebleService.UpdateInmuebleAsync(inmueble);
            if (rows > 0)
                return Results.Ok(rows);

            return Results.NotFound($"Inmueble with id {inmueble.Id} not found to update.");
        }
        catch (Exception ex)
        {
            return Results.InternalServerError($"An error occurred: {ex.Message}");
        }
    }

    public static async Task<Results<Ok<InmuebleReadDTO>, NotFound<string>, ProblemHttpResult>> GetHouseById(IInmuebleService inmuebleService, int id)
    {
        try
        {
            var inmueble = await inmuebleService.GetInmuebleByIdAsync(id);

            if (inmueble == null)
                return TypedResults.NotFound($"Inmueble with id {id} not found.");

            return TypedResults.Ok(inmueble);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem($"An error occurred: {ex.Message}");
        }
    }

    public async static Task<IResult> GetAllHouses(IInmuebleService inmuebleService)
    {
        try
        {
            IEnumerable<CatalogoCasasDTO> datos = await inmuebleService.GetInmueblesAsync();
            return Results.Ok(datos);
        }
        catch (Exception ex)
        {
            return Results.InternalServerError($"An error occurred: {ex.Message}");
        }
    }

    public async static Task<IResult> CreateHuse(InmuebleCreateDTO inmueble, IInmuebleService inmuebleService)
    {
        try
        {
            // VALIDACIONES
            if (string.IsNullOrEmpty(inmueble.NumeroCasa))
            {
                return Results.BadRequest("NumeroCasa is required.");
            }

            // Check by NumeroCasa to avoid duplicates
            int numCasas = await inmuebleService.CountByNumeroCasaAsync(inmueble.NumeroCasa);
            if (numCasas > 0)
            {
                return Results.Conflict($"Numero de Casa {inmueble.NumeroCasa} ya existe.");
            }

            // AGREGA LA NUEVA CAS
            int result = await inmuebleService.CreateInmuebleAsync(inmueble);
            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.InternalServerError($"An error occurred: {ex.Message}");
        }
    }

    public async static Task<IResult> GetEstadosOcupacion(IInmuebleService inmuebleService)
    {
        try
        {
            IEnumerable<EstadoOcupacionReadDTO> datos = await inmuebleService.GetEstadosOcupacionAsync();
            return Results.Ok(datos);
        }
        catch (Exception ex)
        {
            return Results.InternalServerError($"An error occurred: {ex.Message}");
        }
    }

    public static async Task<IResult> GetHouseIdUbicacion(IInmuebleService inmuebleService, int idUbicacion)
    {
        try
        {
            var inmueble = await inmuebleService.GetInmuebleByIdUbicacionAsync(idUbicacion);

            if (inmueble == null)
                return TypedResults.NotFound($"No existen casas con el id {idUbicacion}.");

            return TypedResults.Ok(inmueble);
        }
        catch (Exception ex)
        {
            return TypedResults.Problem($"An error occurred: {ex.Message}");
        }
    }

}
