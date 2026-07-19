using CasaBlanca_API.Models;
using CasaBlanca_API.Models.DTO;

namespace CasaBlanca_API.Interfaces
{
    public interface IInmuebleService
    {
        Task<IEnumerable<ListaCasasDTO>> GetInmueblesAsync();
        Task<IEnumerable<EstadoOcupacionReadDTO>> GetEstadosOcupacionAsync();
        Task<int> CreateInmuebleAsync(InmuebleCreateDTO inmueble);
        Task<int> CountByNumeroCasaAsync(string numeroCasa);
        Task<InmuebleReadDTO?> GetInmuebleByIdAsync(int id);
        Task<int> UpdateInmuebleAsync(UpdateCasaDTO inmueble);
    
    }
}
