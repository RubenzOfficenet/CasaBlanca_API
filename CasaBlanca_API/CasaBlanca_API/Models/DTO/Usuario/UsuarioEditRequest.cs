namespace CasaBlanca_API.Models.DTO.Usuario
{
    public class UsuarioEditRequest
    {
        public int Id { get; set; }

        public string Nombre { get; set; } = null!;

        public string? Apellidos { get; set; }

        public string Email{ get; set; } = null!;

        public string Celular { get; set; } = null!;

        public int? IdRol { get; set; }

        public int? IdInmueble { get; set; }

        public int IdTipoRelacion { get; set; }
    }
}
