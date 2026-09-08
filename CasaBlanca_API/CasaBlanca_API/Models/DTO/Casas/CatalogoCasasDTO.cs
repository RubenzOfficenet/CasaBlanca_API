namespace CasaBlanca_API.Models.DTO.Casas
{
    public class CatalogoCasasDTO
    {
        public int Id { get; set; }
        public string NumeroCasa { get; set; } = string.Empty;
        public int IdUbicacion { get; set; }
        public string NombreUbicacion { get; set; } = string.Empty;
        public decimal CuotaDeMantenimientoBase { get; set; }
        public int IdEstadoOcupacion { get; set; }
        public string EstadoInicialOcupacion { get; set; } = string.Empty;
        public int NumeroHabitantes { get; set; }
        public string? Observaciones { get; set; }
    }
}
