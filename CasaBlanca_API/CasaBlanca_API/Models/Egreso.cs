using System;
using System.Collections.Generic;

namespace CasaBlanca_API.Models;

public partial class Egreso
{
    public int Id { get; set; }

    public DateTime FechaEgreso { get; set; }

    public string? Beneficiario { get; set; }

    public string? Concepto { get; set; }

    public decimal? Monto { get; set; }

    public string? Observaciones { get; set; }

    public bool Borrado { get; set; }

    public DateTime DateAdded { get; set; }

    public DateTime? LastUpdate { get; set; }
}
