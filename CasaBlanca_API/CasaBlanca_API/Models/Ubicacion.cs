using System;
using System.Collections.Generic;

namespace CasaBlanca_API.Models;

public partial class Ubicacion
{
    public int Id { get; set; }

    public string NombreUbicacion { get; set; } = null!;

    public DateTime DatedAdded { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual ICollection<Inmueble> Inmuebles { get; set; } = new List<Inmueble>();
}
