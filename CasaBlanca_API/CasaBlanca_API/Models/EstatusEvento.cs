using System;
using System.Collections.Generic;

namespace CasaBlanca_API.Models;

public partial class EstatusEvento
{
    public int Id { get; set; }

    public string EstatusEvento1 { get; set; } = null!;

    public DateTime DateAdded { get; set; }

    public DateTime? LastUpdated { get; set; }

    public virtual ICollection<EventosIngreso> EventosIngresos { get; set; } = new List<EventosIngreso>();
}
