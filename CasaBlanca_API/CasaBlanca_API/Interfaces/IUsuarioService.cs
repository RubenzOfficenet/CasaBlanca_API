using CasaBlanca_API.Models.DTO.Usuario;

namespace CasaBlanca_API.Interfaces
{
    public interface IUsuarioService
    {
        Task<IEnumerable<GetUsuariosResponse>> GetUsuariosAsync();
        Task<IResult> AddUsuarioAsync(UsuarioRequest usuarioRequest);
        Task<IResult> GetUsuarioByIdAsync(int Id);
        Task<IResult> ActualizarUsuarioAsync(UsuarioEditRequest usuario);
    }
}
