using System;
using System.Collections.Generic;

namespace CasaBlanca_API.Models;

public partial class ConceptoEgresoEvento
{
    public int Id { get; set; }

    public string ConceptoEgreso { get; set; } = null!;

    public DateTime FechaAdd { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual ICollection<EgresoEvento> EgresoEventos { get; set; } = new List<EgresoEvento>();
}
