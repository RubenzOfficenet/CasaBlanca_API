using CasaBlanca_API.Models;
using CasaBlanca_API.Models.DTO;
using CasaBlanca_API.Models.DTO.Casa;
using CasaBlanca_API.Models.DTO.Casas;

namespace CasaBlanca_API.Interfaces
{
    public interface IInmuebleService
    {
        Task<IEnumerable<CatalogoCasasDTO>> GetInmueblesAsync();
        Task<IEnumerable<EstadoOcupacionReadDTO>> GetEstadosOcupacionAsync();
        Task<int> CreateInmuebleAsync(InmuebleCreateDTO inmueble);
        Task<int> CountByNumeroCasaAsync(string numeroCasa);
        Task<InmuebleReadDTO?> GetInmuebleByIdAsync(int id);
        Task<int> UpdateInmuebleAsync(UpdateCasaDTO inmueble);
        Task<IEnumerable<CasaDto>> GetInmuebleByIdUbicacionAsync(int _idUbicacaion);


    }
}
