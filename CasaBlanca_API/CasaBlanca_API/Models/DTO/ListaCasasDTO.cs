namespace CasaBlanca_API.Models.DTO
{
    public class ListaCasasDTO
    {
        public int Id { get; set; }
        public string? NumeroCasa { get; set; }
        public int idubicacion { get; set; }
        public string? nombreubicacion { get; set; }
        public decimal CuotaDeMantenimientoBase { get; set; }
        public int idestadoocupacion { get; set; }
        public string? EstadoInicialOcupacion { get; set; }
        public int? NumeroHabitantes { get; set; }
        public string? Observaciones { get; set; }
        public int idusuario { get; set; }
        public string? nombre { get; set; }
        public string? apellidos { get; set; }
        public int idcasa { get; set; }
        public string rol{ get; set; }
        public int idRol { get; set; }
    }
}
