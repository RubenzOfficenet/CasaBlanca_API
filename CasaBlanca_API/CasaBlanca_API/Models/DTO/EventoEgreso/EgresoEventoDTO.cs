namespace CasaBlanca_API.Models.DTO.EventoEgreso
{
    public class EgresoEventoDTO
    {
        public int IdEventoIngreso { get; set; }
        public int IdConceptoEgresoEvento { get; set; }
        public decimal MontoEgreso { get; set; }
        public DateTime FechaEgreso { get; set; }
        public string? Observaciones { get; set; }
    }
}
