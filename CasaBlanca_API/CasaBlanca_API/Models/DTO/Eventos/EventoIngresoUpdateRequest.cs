namespace CasaBlanca_API.Models.DTO.Eventos
{
    public class EventoIngresoUpdateRequest
    {
        public int Id{ get; set; }
        public int IdInmueble { get; set; }
        public int IdEstatusEvento { get; set; }
        public DateTime FechaEvento { get; set; }
        public string? ReciboNumero { get; set; }
        public DateTime FechaPago { get; set; }
        public decimal Apartado { get; set; }
        public decimal Liquida { get; set; }
        public decimal Luz { get; set; }
        public decimal DepositoGarantia { get; set; }
        public decimal LimpiezaDomingo { get; set; }
        public decimal RentaInmobiliario { get; set; }
    }
}
