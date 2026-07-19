namespace CasaBlanca_API.Models.DTO.Usuario
{
    public class GetUsuariosResponse
    {

        public int IdUsuario { get; set; }

        public string? NombreUsuario { get; set; }

        public string? ApellidosUsuario { get; set; }

        public string? EmailUsuario { get; set; }

        public string? CelularUsuario { get; set; }

        public int Idrol { get; set; }

        public string? Rol { get; set; }

        public int IdInmueble { get; set; }

        public string? NumeroCasa { get; set; }

        public int IdEstatus { get; set; }

        public string? Estatus { get; set; }

    }
}
