namespace CasaBlanca_API.Models.DTO.ResumenAnalitico
{
    public class ResumenAnaliticoResponse
    {
        public DateTime FechaDeOperacion { get; set; }
        public string Concepto { get; set; } = string.Empty;
        public string? Ubicacion { get; set; }
        public string? Casa { get; set; }
        public string Tipo { get; set; } = string.Empty;
        public decimal Monto { get; set; }
        public decimal TotalIngresos { get; set; }
        public decimal TotalEgresos { get; set; }
    }
}
