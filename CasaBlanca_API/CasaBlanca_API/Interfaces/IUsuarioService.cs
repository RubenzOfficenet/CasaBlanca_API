using CasaBlanca_API.Models.DTO.Usuario;

namespace CasaBlanca_API.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<GetUsuariosResponse>> GetUsuariosAsync();
    }
}
