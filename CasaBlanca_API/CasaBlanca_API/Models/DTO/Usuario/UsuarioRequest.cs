namespace CasaBlanca_API.Models.DTO.Usuario
{
    public class UsuarioRequest
    {
        public string NombreUsuario { get; set; } = null!;

        public string? ApellidosUsuario { get; set; }

        public string EmailUsuario { get; set; } = null!;

        public string? Password { get; set; }

        public string CelularUsuario { get; set; } = null!;

        public int? IdRol { get; set; }

        public int? IdInmueble { get; set; }
    }
}
