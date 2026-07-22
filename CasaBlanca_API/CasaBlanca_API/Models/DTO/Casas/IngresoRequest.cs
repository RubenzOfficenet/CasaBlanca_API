namespace CasaBlanca_API.Models.DTO.Casas
{
    public class IngresoRequest
    {
        public int IdCasa { get; set; }

        public DateTime FechaRecepcion { get; set; }

        public string NumeroRecibo { get; set; } = string.Empty;

        public int IdConcepto { get; set; }

        public DateTime FechaConcepto { get; set; }

        public decimal Monto { get; set; }

        public string? Observaciones { get; set; }
    }
}
