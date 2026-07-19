using System;
using System.Collections.Generic;

namespace CasaBlanca_API.Models;

public partial class RolUser
{
    public int Id { get; set; }

    public string Rol { get; set; } = null!;

    public DateTime DateAdded { get; set; }

    public DateTime? DateUpdated { get; set; }

    public virtual ICollection<Usuario> Usuarios { get; set; } = new List<Usuario>();
}
