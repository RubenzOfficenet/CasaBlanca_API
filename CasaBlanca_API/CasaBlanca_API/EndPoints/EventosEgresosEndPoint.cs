using AutoMapper;
using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models;
using CasaBlanca_API.Models.DTO.Even_toEgreso;
using CasaBlanca_API.Models.DTO.EventoEgreso;
using CasaBlanca_API.Models.DTO.Eventos;

namespace CasaBlanca_API.EndPoints
{
    public static class EventosEgresosEndPoint
    {
        public static void ConfigureEventosEgresosEndPoint(this WebApplication app)
        {
            app.MapGet("/api/GetEventoEgresos", GetEventoEgresos).WithName("GetEventoEgresos").Produces<IEnumerable<EventoResponse>>(StatusCodes.Status200OK).Produces(StatusCodes.Status500InternalServerError);
            app.MapGet("/api/GetListaEventoEgresos", GetListaEventoEgresos).WithName("GetListaEventoEgresos").Produces<IEnumerable<EventoResponse>>(StatusCodes.Status200OK).Produces(StatusCodes.Status500InternalServerError);
            app.MapPost("/api/AddEventoEgreso", AddEventoEgreso).WithName("AddEventoEgreso").Produces<int>(StatusCodes.Status200OK).Produces(StatusCodes.Status500InternalServerError);
        }

        private static async Task<IResult> GetEventoEgresos(ICatalogosService eventoService)
        {
            try
            {
                IEnumerable<ConceptoEventoEgresoDTO> result = await eventoService.GetCatalogoEventosEgreso();

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

        private static async Task<IResult> AddEventoEgreso(EgresoEventoDTO eventoRequest, IEgresoEventoService egresoEventoService, IMapper mapper)
        {
            try
            {
                int result = await egresoEventoService.AddEgresoEventoAsync(eventoRequest);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    title: "Error al agregar el Egreso del Evento",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        private static async Task<IResult> GetListaEventoEgresos(int IdEventoEgreso, IEgresoEventoService egresoEventoService)
        {
            try
            {
                IEnumerable<EgresoEventoResponse> result = await egresoEventoService.GetListaEgresoEventoAsync(IdEventoEgreso);

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


}  // and class
