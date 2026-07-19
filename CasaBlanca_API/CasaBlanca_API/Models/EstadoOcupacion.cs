using System;
using System.Collections.Generic;

namespace CasaBlanca_API.Models;

public partial class EstadoOcupacion
{
    public int Id { get; set; }

    public string? EstadoInicialOcupacion { get; set; }

    public DateTime DateAdded { get; set; }

    public DateTime? LastUpadded { get; set; }

    public virtual ICollection<Inmueble> Inmuebles { get; set; } = new List<Inmueble>();
}
