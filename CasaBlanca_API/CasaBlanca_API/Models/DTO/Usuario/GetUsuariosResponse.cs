namespace CasaBlanca_API.Models.DTO.Usuario
{
    public class GetUsuariosResponse
    {
        // Datos de usuario
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellidos { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Celular { get; set; } = string.Empty;
        // Datos de casa_usuario
        public int IdRol { get; set; }
        public string Rol { get; set; } = string.Empty;
        public int IdCasa { get; set; }
        // Datos de inmueble
        public string NumeroCasa { get; set; } = string.Empty;
        public int IdUbicacion { get; set; }
        // Datos de ubicacion
        public string NombreUbicacion { get; set; } = string.Empty;
        // Datos de tiporelacion
        public int IdTipoRelacion { get; set; }
        public string TipoRelacion { get; set; } = string.Empty;
        // Datos de estatus
        public int IdEstatus { get; set; }
        public string Estatus { get; set; } = string.Empty;
    }


}  // end class
