using CasaBlanca_API.Models.DTO.Egresos;

namespace CasaBlanca_API.Interfaces
{
    public interface IEgresoService
    {
        Task<IEnumerable<EgresoResponse>> GetAllEgresosAsync(int year, int month, CancellationToken cancellationToken = default);
        Task<int> AddEgresoAsync(EgresoRequest egresoRequest, CancellationToken cancellationToken = default);
        Task<EgresoResponse?> GetEgresoByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<int> UpdateEgresoAsync(int id, EgresoRequest egresoRequest, CancellationToken cancellationToken = default);
        Task<int> DeleteEgresoAsync(int id, CancellationToken cancellationToken = default);

    }
}
