using AutoMapper;
using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models.DTO.Casas;
using CasaBlanca_API.Models.DTO.Egresos;
using CasaBlanca_API.Models.DTO.Ingresos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CasaBlanca_API.Implementations;

public static class EgresosEndPoints
{
    public static void ConfigureEgresosEndPoints(this WebApplication app)
    {
        app.MapPost("/api/AddEgreso", AddEgreso).WithName("AddEgreso").Produces<int>(StatusCodes.Status200OK);
        app.MapGet("/api/GetEgresos", GetEgresos).WithName("GetEgresos").Produces<IEnumerable<EgresoResponse>>(StatusCodes.Status200OK).Produces(StatusCodes.Status500InternalServerError);
        app.MapPut("/api/UpdateEgreso", UpdateEgreso).WithName("UpdateEgreso").Produces<int>(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status500InternalServerError);
        app.MapGet("/api/GetEgresoById/{id}", GetEgresoById).WithName("GetEgresoById").Produces<EgresoResponse>(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status500InternalServerError);
        app.MapDelete("/api/DeleteEgreso/{id}", DeleteEgreso).WithName("DeleteEgreso").Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status500InternalServerError);
    }

    private static async Task<IResult> AddEgreso(EgresoRequest egresoRequest, IEgresoService egresoService, IMapper mapper)
    {
        try
        {
            int result = await egresoService.AddEgresoAsync(egresoRequest);

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.Problem(ex.Message);
        }
    }

    private static async Task<IResult> GetEgresos(IEgresoService egresoService, int year, int month)
    {
        try
        {
            var result = await egresoService.GetAllEgresosAsync(year, month);

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                detail: ex.Message,
                title: "Error al obtener los egresos",
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    static async Task<IResult> UpdateEgreso(int id, EgresoRequest egreso, IEgresoService egresoService)
    {
        try
        {
            var result = await egresoService.UpdateEgresoAsync(id, egreso);

            if (result == 0)
            {
                return Results.NotFound(new { mensaje = $"No se encontró el egreso con Id {id}" });
            }

            return Results.Ok(result);
        }
        catch (Exception ex)
        {
            return Results.Problem(
                     detail: ex.Message,
                     title: "Error al actualizar el egreso",
                     statusCode: StatusCodes.Status500InternalServerError);
        }
    }

    static async Task<IResult> GetEgresoById(int id, IEgresoService egresoService)
    {
        try
        {
            EgresoResponse? egreso = await egresoService.GetEgresoByIdAsync(id);

            return egreso is null
                ? Results.NotFound()
                : Results.Ok(egreso);
        }
        catch (Exception)
        {
            return Results.StatusCode(StatusCodes.Status500InternalServerError);
        }
    }

    static async Task<IResult> DeleteEgreso(int id, IEgresoService egresoService)
    {
        try
        {
            var egresoExistente = await egresoService.GetEgresoByIdAsync(id);

            if (egresoExistente is null)
            {
                return Results.NotFound($"No se encontró el egreso con id {id}.");
            }

            int result = await egresoService.DeleteEgresoAsync(id);

            if (result == 0)
            {
                return Results.NotFound($"No se pudo eliminar el egreso con id {id}.");
            }

            return Results.Ok(new { mensaje = "Egreso eliminado correctamente." });
        }
        catch (Exception ex)
        {
            return Results.Problem(
                detail: ex.Message,
                statusCode: StatusCodes.Status500InternalServerError);
        }
    }
}
