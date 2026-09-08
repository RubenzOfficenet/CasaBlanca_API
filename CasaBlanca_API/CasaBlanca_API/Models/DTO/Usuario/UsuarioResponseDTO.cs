namespace CasaBlanca_API.Models.DTO.Usuario
{
    public class UsuarioResponseDTO
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        public int IdEstatus { get; set; }
        public string Estatus { get; set; } = string.Empty;
        public int IdInmueble { get; set; }
        public string NumeroCasa { get; set; } = string.Empty; 
        public int IdUbicacion { get; set; }
        public string NombreUbicacion { get; set; } = string.Empty;
        public int IdRol { get; set; }
        public string Rol { get; set; } = string.Empty;
        public bool Borrado { get; set; }
    }
}
