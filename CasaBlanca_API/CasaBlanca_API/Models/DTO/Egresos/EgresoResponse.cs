
namespace CasaBlanca_API.Models.DTO.Egresos
{
    public class EgresoResponse
    {
        public int Id { get; set; }
        public DateTime FechaEgreso { get; set; }
        public string? Beneficiario { get; set; }
        public string? Concepto { get; set; }
        public decimal Monto { get; set; }
        public string? Observaciones { get; set; }
        public decimal totalMonto { get; set; }
    }
}
