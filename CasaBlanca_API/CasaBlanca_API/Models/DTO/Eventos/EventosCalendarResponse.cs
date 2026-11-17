namespace CasaBlanca_API.Models.DTO.Eventos
{
    public class EventosCalendarResponse
    {
        public int Id { get; set; }
        public int IdInmueble { get; set; }
        public int IdEstatusEvento { get; set; }
        public string EstatusEvento { get; set; } = string.Empty;
        public DateTime FechaEvento { get; set; }
        public string? ReciboNumero { get; set; }
        public DateTime? FechaPago { get; set; }
        public decimal Apartado { get; set; }
        public decimal Liquida { get; set; }
        public decimal Luz { get; set; }
        public decimal DepositoGarantia { get; set; }
        public decimal LimpiezaDomingo { get; set; }
        public decimal RentaInmobiliario { get; set; }
        public string? Comentarios { get; set; }
        public int IdCasa { get; set; }
        public string NumeroCasa { get; set; } = string.Empty;
        public int IdUbicacion { get; set; }
        public string NombreUbicacion { get; set; } = string.Empty;
        public bool Borrado { get; set; }
        public string? nombre { get; set; }
        public string? apellidos { get; set; }
        public decimal apartado { get; set; }
        public decimal liquida { get; set; }


    }
}
