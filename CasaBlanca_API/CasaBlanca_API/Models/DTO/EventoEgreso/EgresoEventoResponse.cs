namespace CasaBlanca_API.Models.DTO.EventoEgreso
{
    public class EgresoEventoResponse
    {
        public int Id { get; set; }
        public int IdEventoIngreso { get; set; }
        public int IdUbicacion { get; set; }
        public DateTime FechaEvento { get; set; }
        public string NombreUbicacion { get; set; } = string.Empty;
        public string NumeroCasa { get; set; } = string.Empty;
        public string ConceptoEgreso { get; set; } = string.Empty;
        public decimal MontoEgreso { get; set; }
        public DateTime FechaEgreso { get; set; }
        public string? Observaciones { get; set; }
    }
}
