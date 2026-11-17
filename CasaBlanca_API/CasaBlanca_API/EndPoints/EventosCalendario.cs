using CasaBlanca_API.Implementations;
using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models.DTO.Egresos;
using Microsoft.AspNetCore.Mvc;

namespace CasaBlanca_API.EndPoints
{
    public static class EventosCalendario
    {

        public static void EventosCalendarioEndPoints(this WebApplication app)
        {
            app.MapGet("/api/GetEventosCalendario", GetEventosCalendario).WithName("GetEventosCalendario").Produces<IEnumerable<EgresoResponse>>(StatusCodes.Status200OK).Produces(StatusCodes.Status500InternalServerError);
        }


        private static async Task<IResult> GetEventosCalendario([FromServices] IEventosCalendarioService eventosCalendarioService,int year, int month)
        {
            try
            {
                var result = await eventosCalendarioService.GetEventosCalendarioAsync(year, month);

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
    }
}
