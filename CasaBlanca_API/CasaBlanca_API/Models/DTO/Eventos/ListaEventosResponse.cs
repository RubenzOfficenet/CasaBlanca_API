namespace CasaBlanca_API.Models.DTO.Eventos
{
    public class ListaEventosResponse
    {
        public int Id { get; set; }
        public int IdInmueble { get; set; }
        public string NumeroCasa { get; set; } = string.Empty;
        public int IdUbicacion { get; set; }
        public string NombreUbicacion { get; set; } = string.Empty;
        public int IdEstatusEvento { get; set; }
        public string EstatusEvento { get; set; } = string.Empty;
        public DateTime FechaEvento { get; set; }
        public string? ReciboNumero { get; set; }

        // Propiedades de Usuario (pueden ser nulas debido al LEFT JOIN)
        public string? Nombre { get; set; }
        public string? Apellidos { get; set; }
        public decimal apartado { get; set; }
        public decimal liquida { get; set; }


        // Propiedad calculada útil para mostrar el nombre completo en la UI
        public string NombreCompletoUsuario =>
            !string.IsNullOrWhiteSpace(Nombre)
                ? $"{Nombre} {Apellidos}".Trim()
                : "Sin Asignar";
    }
}
