using System;
using System.Collections.Generic;

namespace CasaBlanca_API.Models;

public partial class Estatus
{
    public int Id { get; set; }

    public string Estatus1 { get; set; } = null!;

    public DateTime DatedAdded { get; set; }

    public DateTime? DateUpdated { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
