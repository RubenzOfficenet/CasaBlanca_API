using CasaBlanca_API.Models;
using CasaBlanca_API.Models.DTO.EventoEgreso;

namespace CasaBlanca_API.Interfaces
{
    public interface IEgresoEventoService
    {
        Task<int> AddEgresoEventoAsync(EgresoEventoDTO egresoEventoRequest, CancellationToken cancellationToken = default);
        Task<IEnumerable<EgresoEventoResponse>> GetListaEgresoEventoAsync(int idEventoEgreso, CancellationToken cancellationToken = default);
    }
}
