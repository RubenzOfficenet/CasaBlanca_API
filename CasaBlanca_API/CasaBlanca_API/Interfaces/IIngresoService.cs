using CasaBlanca_API.Models.DTO.Casas;
using CasaBlanca_API.Models.DTO.Ingresos;

namespace CasaBlanca_API.Interfaces
{
    public interface IIngresoService
    {
        Task<int> AddIngresoAsync(IngresoRequest ingresoRequest);
        Task<IEnumerable<IngresoResponse>> GetAllIngresosAsync(int year, int month);
    }
}
