using CasaBlanca_API.Models.DTO.Catalogo;
using CasaBlanca_API.Models.DTO.Rol;
using CasaBlanca_API.Models.DTO.Ubicacion;

namespace CasaBlanca_API.Interfaces
{
    public interface ICatalogosService
    {
        Task<IEnumerable<RolDTO>> GetAllRolAsync();
        Task<IEnumerable<CatalogoIngresoResponse>> GetAllConceptoIngresosAsync();
        Task<IEnumerable<UbicacionResponseDTO>> GetAllUbicaciones();
        Task<IEnumerable<EstatusEventoResponse>> GetAllEstatusEvento();
    }
}
