using CasaBlanca_API.Models.DTO.Eventos;

namespace CasaBlanca_API.Interfaces
{
    public interface IEventosCalendarioService
    {
        Task<IEnumerable<ListaEventosResponse>> GetEventosCalendarioAsync(int year, int month);
    }
}
