namespace CasaBlanca_API.Models.DTO.Ingresos
{
    public class IngresoResponse
    {
        public int Id { get; set; }

        public string NumeroCasa { get; set; } = string.Empty;

        public string NombreTitular { get; set; } = string.Empty;

        public string NombreOcupante { get; set; } = string.Empty;

        public DateTime FechaRecepcion { get; set; }

        public string NumeroRecibo { get; set; } = string.Empty;

        public string Concepto { get; set; } = string.Empty;

        public DateTime FechaConcepto { get; set; }

        public decimal Monto { get; set; }

        public string? Observaciones { get; set; }

        public decimal totalMonto { get; set; }
    }
}
