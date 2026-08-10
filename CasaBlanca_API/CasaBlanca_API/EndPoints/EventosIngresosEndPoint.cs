using AutoMapper;
using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models.DTO.Casas;
using CasaBlanca_API.Models.DTO.Egresos;
using CasaBlanca_API.Models.DTO.Eventos;
using Microsoft.AspNetCore.Mvc;

namespace CasaBlanca_API.EndPoints
{
    public static class EventosIngresosEndPoint
    {
        public static void ConfigureEventosIngresosEndPoint(this WebApplication app)
        {   
            app.MapGet("/api/GetEventoingresos", GetEventoingresos)
                .WithName("GetEventoingresos")
                .Produces<IEnumerable<EventoResponse>>(StatusCodes.Status200OK)
                .Produces(StatusCodes.Status500InternalServerError); 

            app.MapPost("/api/AddEventoIngreso", AddEventoIngreso).WithName("AddEventoIngreso").Produces<int>(StatusCodes.Status200OK).Produces(StatusCodes.Status500InternalServerError);

            //app.MapPut("/api/UpdateEgreso", UpdateEgreso).WithName("UpdateEgreso").Produces<int>(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status500InternalServerError);
            //app.MapGet("/api/GetEgresoById/{id}", GetEgresoById).WithName("GetEgresoById").Produces<EgresoResponse>(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status500InternalServerError);
            //app.MapDelete("/api/DeleteEgreso/{id}", DeleteEgreso).WithName("DeleteEgreso").Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status500InternalServerError);
        }
    
        private static async Task<IResult> GetEventoingresos(IEventoService eventoService, [AsParameters] EventoGetAllRequest eventoFiltro)
        {
            try
            {
                IEnumerable<EventoResponse> result = await eventoService.GetAllEventsAsync(eventoFiltro);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    title: "Error al obtener los Eventos",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        private static async Task<IResult> AddEventoIngreso(EventoAddRequest eventoRequest, IEventoService eventoService, IMapper mapper)
        {
            try
            {
                int result = await eventoService.AddEventoAsync(eventoRequest);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    title: "Error al agregar el Evento",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }

    }
}
