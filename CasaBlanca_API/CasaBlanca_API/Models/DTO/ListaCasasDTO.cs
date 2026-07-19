namespace CasaBlanca_API.Models.DTO
{
    public class ListaCasasDTO
    {
        public int Id { get; set; }

        public string? NumeroCasa { get; set; }
        public decimal CuotaDeMantenimientoBase { get; set; }
        public int? EstadoOcupacion { get; set; }

        public string? EstadoInicialOcupacion { get; set; }

        public string? NombreTitular { get; set; }

        public string? ApellidosTitular { get; set; }
        public string? CelularTitular { get; set; }

        public string? EmailTitular { get; set; }


        public string? NombreOcupante { get; set; }

        public string? ApellidosOcupante { get; set; }

        public string? EmailOcupante { get; set; }

        public string? CelularOcupante { get; set; }

        public int? NumeroHabitantes { get; set; }

        public string? Observaciones { get; set; }
    }
}
