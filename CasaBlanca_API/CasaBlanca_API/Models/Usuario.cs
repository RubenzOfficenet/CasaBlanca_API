using System;
using System.Collections.Generic;

namespace CasaBlanca_API.Models;

public partial class Usuario
{
    public int Id { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string? ApellidosUsuario { get; set; }

    public string EmailUsuario { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string CelularUsuario { get; set; } = null!;

    public int? IdRol { get; set; }

    public int? IdInmueble { get; set; }

    public int? IdEstatus { get; set; }

    public DateTime DateAdded { get; set; }

    public DateTime? DateUpdated { get; set; }

    public virtual Estatus? IdEstatusNavigation { get; set; }

    public virtual Inmueble? IdInmuebleNavigation { get; set; }

    public virtual RolUser? IdRolNavigation { get; set; }
}
