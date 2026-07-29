using System;
using System.Collections.Generic;

namespace CasaBlanca_API.Models;

public partial class Concepto
{
    public int Id { get; set; }

    public string Concepto1 { get; set; } = null!;

    public string TipoConcepto { get; set; } = null!;

    public DateTime DateAdded { get; set; }

    public DateTime? DateUpdated { get; set; }

    public virtual ICollection<Ingreso> Ingresos { get; set; } = new List<Ingreso>();
}
