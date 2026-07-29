namespace CasaBlanca_API.Models.DTO.Ingresos
{
    public class IngresoRequestDTO
    {
        public int Id { get; set; }
        public int IdCasa { get; set; }
        public DateTime FechaRecepcion { get; set; }
        public int NumeroRecibo { get; set; }
        public int IdConcepto { get; set; }
        public DateTime FechaConcepto { get; set; }
        public decimal Monto { get; set; }
        public string? Observaciones { get; set; }
    }
}
