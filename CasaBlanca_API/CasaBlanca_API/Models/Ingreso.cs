using System;
using System.Collections.Generic;

namespace CasaBlanca_API.Models;

public partial class Ingreso
{
    public int Id { get; set; }

    public int IdCasa { get; set; }

    public DateTime FechaRecepcion { get; set; }

    public int NumeroRecibo { get; set; }

    public int IdConcepto { get; set; }

    public DateTime FechaConcepto { get; set; }

    public decimal Monto { get; set; }

    public string? Observaciones { get; set; }

    public bool? Borrado { get; set; }

    public DateTime DateAdded { get; set; }

    public DateTime? DateUpdated { get; set; }

    public virtual Inmueble IdCasaNavigation { get; set; } = null!;

    public virtual Concepto IdConceptoNavigation { get; set; } = null!;
}
