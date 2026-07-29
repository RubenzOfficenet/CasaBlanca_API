using AutoMapper;
using Azure.Core;
using CasaBlanca_API.Implementations;
using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models.DTO.Casas;
using CasaBlanca_API.Models.DTO.Ingresos;
using System.Data;

namespace CasaBlanca_API.EndPoints
{
    public static class IngresosEndPoints
    {
        public static void ConfigureIngresosEndPoints(this WebApplication app)
        {
            app.MapPost("/api/AddIngreso", AddIngreso).WithName("AddIngreso").Produces<int>(StatusCodes.Status200OK);
            app.MapGet("/api/GetIngresos", GetIngresos).WithName("GetIngresos").Produces<IEnumerable<IngresoResponse>>(StatusCodes.Status200OK).Produces(StatusCodes.Status500InternalServerError);
            app.MapPost("/api/actualizaIngresos", ActualizaIngresos).WithName("ActualizaIngresos").Produces<int>(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status500InternalServerError);
            app.MapGet("/api/GetIngresoById/{id}", GetIngresoById).WithName("GetIngresoById").Produces<IngresoResponse>(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status500InternalServerError);
            app.MapPut("/api/UpdateIngreso", UpdateIngreso).WithName("UpdateIngreso").Produces<IngresoResponse>(StatusCodes.Status200OK).Produces(StatusCodes.Status400BadRequest).Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status500InternalServerError);
            app.MapDelete("/api/DeleteIngreso/{id}", DeleteIngreso).WithName("DeleteIngreso").Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status500InternalServerError);
        }

        private static async Task<IResult> AddIngreso(IngresoRequest ingresoRequest, IIngresoService ingresoService, IMapper mapper)
        {
            try
            {
                int result = await ingresoService.AddIngresoAsync(ingresoRequest);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> GetIngresos(IIngresoService ingresoService, int year, int month)
        {
            try
            {
                var result = await ingresoService.GetAllIngresosAsync(year, month);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    title: "Error al obtener los ingresos",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        static async Task<IResult> ActualizaIngresos(IngresoUpdateRequest ingreso, IIngresoService ingresoService)
        {
            try
            {
                var result = await ingresoService.ActualizarIngresoAsync(ingreso);

                if (result == 0)
                {
                    return Results.NotFound(new { mensaje = $"No se encontró el ingreso con Id {ingreso.Id}" });
                }

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                         detail: ex.Message,
                         title: "Error al actualizar el ingreso",
                         statusCode: StatusCodes.Status500InternalServerError);
            }

            

            
        }

        static async Task<IResult> GetIngresoById(int id, IIngresoService ingresoService)
        {
            try
            {
                var ingreso = await ingresoService.GetIngresoByIdAsync(id);

                return ingreso is null
                    ? Results.NotFound()
                    : Results.Ok(ingreso);
            }
            catch (Exception)
            {
                return Results.StatusCode(StatusCodes.Status500InternalServerError);
            }
        }

        static async Task<IResult> UpdateIngreso(IngresoUpdateRequest request, IIngresoService ingresoService)
        {
            try
            {
                IngresoUpdateDTO? ingresos = await ingresoService.GetIngresoByIdAsync(request.Id);

                if (ingresos is null)
                {
                    return Results.NotFound($"No se encontró el ingreso con id {request.Id}.");
                }

                var ingresoRequestDTO = new IngresoRequestDTO
                {
                    Id = request.Id,
                    IdCasa = request.IdCasa,
                    FechaRecepcion = request.FechaRecepcion,
                    NumeroRecibo = request.NumeroRecibo,
                    IdConcepto = request.IdConcepto,
                    FechaConcepto = request.FechaConcepto,
                    Monto = request.Monto,
                    Observaciones = request.Observaciones
                };

                int result = await ingresoService.UpdateIngresoAsync(ingresoRequestDTO);

                if (result == 0)
                {
                    return Results.NotFound($"No se pudo actualizar el ingreso con id {request.Id}.");
                }

                return Results.Ok(new { mensaje = "Ingreso actualizado correctamente.", filasAfectadas = result });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError);
            }

        }

        static async Task<IResult> DeleteIngreso(int id,IIngresoService ingresoService)
        {
            try
            {
                var ingresoExistente = await ingresoService.GetIngresoByIdAsync(id);

                if (ingresoExistente is null)
                {
                    return Results.NotFound($"No se encontró el ingreso con id {id}.");
                }

                int result = await ingresoService.DeleteIngresoAsync(id);

                if (result == 0)
                {
                    return Results.NotFound($"No se pudo eliminar el ingreso con id {id}.");
                }

                return Results.Ok(new { mensaje = "Ingreso eliminado correctamente." });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

    }
}