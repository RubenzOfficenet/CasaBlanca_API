using CasaBlanca_API.Models.DTO.Rol;

namespace CasaBlanca_API.Interfaces
{
    public interface ICatalogosService
    {
        Task<IEnumerable<RolDTO>> GetAllRolAsync();
    }
}
