namespace CasaBlanca_API.Models.DTO.Ingresos
{
    public class IngresoUpdateDTO
    {
        public int Id { get; set; }
        public int IdCasa { get; set; }
        public string? NumeroCasa { get; set; }
        public string? NombreTitular { get; set; }
        public string? NombreOcupante { get; set; }
        public string? FechaRecepcion { get; set; }
        public string? NumeroRecibo { get; set; }
        public int IdConcepto { get; set; }
        public string? Concepto { get; set; }
        public string? FechaConcepto { get; set; } 
        public decimal Monto { get; set; }
        public string? Observaciones { get; set; }
    }
}
