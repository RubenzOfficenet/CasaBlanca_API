using CasaBlanca_API.Models.DTO.ResumenAnalitico;

namespace CasaBlanca_API.Interfaces
{
    public interface IEgresoAnaliticoService
    {
        Task<IEnumerable<ResumenAnaliticoResponse>> GetResumenAnaliticoAsync();
    }
}
