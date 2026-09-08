namespace CasaBlanca_API.Models.DTO.Usuario
{
    public class UsuarioResponse
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public int IdEstatus { get; set; }
        public int? IdInmueble { get; set; } 
        public int IdRol { get; set; }
        public bool Borrado { get; set; }
        public DateTime DateAdded { get; set; }
        public DateTime? LastUpdate { get; set; }
    }
}
