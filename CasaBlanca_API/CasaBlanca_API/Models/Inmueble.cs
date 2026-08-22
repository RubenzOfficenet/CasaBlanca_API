using System;
using System.Collections.Generic;

namespace CasaBlanca_API.Models;

public partial class Inmueble
{
    public int Id { get; set; }

    public string? NumeroCasa { get; set; }

    public int IdUbicacion { get; set; }

    public decimal CuotaDeMantenimientoBase { get; set; }

    public int? EstadoOcupacion { get; set; }

    public string? NombreTitular { get; set; }

    public string? ApellidosTitular { get; set; }

    public string? EmailTitular { get; set; }

    public string? CelularTitular { get; set; }

    public string? NombreOcupante { get; set; }

    public string? ApellidosOcupante { get; set; }

    public string? EmailOcupante { get; set; }

    public string? CelularOcupante { get; set; }

    public int? NumeroHabitantes { get; set; }

    public string? Observaciones { get; set; }

    public string Usuario { get; set; } = null!;

    public string Password { get; set; } = null!;

    public DateTime DateAdded { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual EstadoOcupacion? EstadoOcupacionNavigation { get; set; }

    public virtual ICollection<EventosIngreso> EventosIngresos { get; set; } = new List<EventosIngreso>();

    public virtual Ubicacion IdUbicacionNavigation { get; set; } = null!;

    public virtual ICollection<Ingreso> Ingresos { get; set; } = new List<Ingreso>();

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
