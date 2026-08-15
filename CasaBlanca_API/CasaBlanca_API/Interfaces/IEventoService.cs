using CasaBlanca_API.Models.DTO.Eventos;

namespace CasaBlanca_API.Interfaces
{
    public interface IEventoService
    {
        Task<IEnumerable<EventoResponse>> GetAllEventsAsync(EventoGetAllRequest eventoFiltro);
        Task<int> AddEventoAsync(EventoAddRequest eventoRequest);
        Task<EventoResponse> GetEventoingresosById(int idEvento);
        Task<int> UpdateEventoingreso(EventoIngresoUpdateRequest eventoIngresoUpdateRequest);
    }
}
