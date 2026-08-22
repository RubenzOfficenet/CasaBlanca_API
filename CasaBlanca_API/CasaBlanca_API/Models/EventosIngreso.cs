using System;
using System.Collections.Generic;

namespace CasaBlanca_API.Models;

public partial class EventosIngreso
{
    public int Id { get; set; }

    public int IdInmueble { get; set; }

    public int IdEstatusEvento { get; set; }

    public DateTime FechaEvento { get; set; }

    public int ReciboNumero { get; set; }

    public DateTime FechaPago { get; set; }

    public decimal Apartado { get; set; }

    public decimal Liquida { get; set; }

    public decimal Luz { get; set; }

    public decimal DepositoGarantia { get; set; }

    public decimal LimpiezaDomingo { get; set; }

    public decimal RentaInmobiliario { get; set; }

    public bool Borrado { get; set; }

    public DateTime DateAdded { get; set; }

    public DateTime? LastUpdate { get; set; }

    public virtual EstatusEvento IdEstatusEventoNavigation { get; set; } = null!;

    public virtual Inmueble IdInmuebleNavigation { get; set; } = null!;
}
