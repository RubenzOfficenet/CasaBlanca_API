using CasaBlanca_API.Models.DTO.Catalogo;
using CasaBlanca_API.Models.DTO.Rol;

namespace CasaBlanca_API.Interfaces
{
    public interface ICatalogosService
    {
        Task<IEnumerable<RolDTO>> GetAllRolAsync();
        Task<IEnumerable<CatalogoIngresoResponse>> GetAllConceptoIngresosAsync();
    }
}
