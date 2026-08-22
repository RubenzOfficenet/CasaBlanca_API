using System;
using System.Collections.Generic;

namespace CasaBlanca_API.Models;

public partial class EgresoEvento
{
    public int Id { get; set; }

    public int IdConceptoEgresoEvento { get; set; }

    public decimal MontoEgreso { get; set; }

    public DateTime FechaEgreso { get; set; }

    public string? Observaciones { get; set; }

    public DateTime AddDate { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual ConceptoEgresoEvento IdConceptoEgresoEventoNavigation { get; set; } = null!;
}
