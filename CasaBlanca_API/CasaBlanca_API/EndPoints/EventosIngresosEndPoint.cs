using AutoMapper;
using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models.DTO.Casas;
using CasaBlanca_API.Models.DTO.Egresos;
using CasaBlanca_API.Models.DTO.Eventos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace CasaBlanca_API.EndPoints
{
    public static class EventosIngresosEndPoint
    {
        public static void ConfigureEventosIngresosEndPoint(this WebApplication app)
        {   
            app.MapGet("/api/GetEventoingresos", GetEventoingresos).WithName("GetEventoingresos").Produces<IEnumerable<EventoResponse>>(StatusCodes.Status200OK).Produces(StatusCodes.Status500InternalServerError); 
            app.MapPost("/api/AddEventoIngreso", AddEventoIngreso).WithName("AddEventoIngreso").Produces<int>(StatusCodes.Status200OK).Produces(StatusCodes.Status500InternalServerError);
            app.MapGet("/api/GetEventoingresosById", GetEventoingresosById).WithName("GetEventoingresosById").Produces<IEnumerable<EventoResponse>>(StatusCodes.Status200OK).Produces(StatusCodes.Status500InternalServerError);
            app.MapPut("/api/UpdateEventoingreso", UpdateEventoingreso).WithName("UpdateEventoingreso").Produces<int>(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status500InternalServerError);
            app.MapPut("/api/DeleteEventoIngreso", DeleteEventoIngreso).WithName("DeleteEventoIngreso").Produces<int>(StatusCodes.Status200OK).Produces(StatusCodes.Status404NotFound).Produces(StatusCodes.Status500InternalServerError);
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

        private static async Task<IResult> GetEventoingresosById(int IdEvento, IEventoService eventoService)
        {
            try
            {
                EventoResponse result = await eventoService.GetEventoingresosById(IdEvento);

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

        private static async Task<IResult> UpdateEventoingreso([FromBody] EventoIngresoUpdateRequest eventoIngresoUpdateRequest, IEventoService eventoService)
        {
            try
            {
                int result = await eventoService.UpdateEventoingreso(eventoIngresoUpdateRequest);

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

        private static async Task<IResult> DeleteEventoIngreso(int idEvento, IEventoService eventoService)
        {
            try
            {
                int result = await eventoService.DeleteEventoIngreso(idEvento);

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



    }
}
