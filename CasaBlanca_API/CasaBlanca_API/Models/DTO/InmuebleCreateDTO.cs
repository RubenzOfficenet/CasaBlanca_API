
namespace CasaBlanca_API.Models.DTO;

public class InmuebleCreateDTO
{
    public string? NumeroCasa { get; set; }

    public int IdUbicacion { get; set; }

    public decimal CuotaDeMantenimientoBase { get; set; }

    public int? IdEstadoOcupacion { get; set; }

    public int NumeroHabitantes { get; set; }

    public string? Observaciones { get; set; } 
}


