namespace CasaBlanca_API.Models.DTO.Usuario
{
    public class UsuarioRequest
    {
        public string NombreUsuario { get; set; } = null!;

        public string? ApellidosUsuario { get; set; }

        public string EmailUsuario { get; set; } = null!;

        public string CelularUsuario { get; set; } = null!;

        public string Password { get; set; } = string.Empty;

        public int IdEstatus { get; set; }

        public int? IdRol { get; set; }

        public int? IdInmueble { get; set; }

        public int IdTipoRelacion { get; set; }
    }
}
