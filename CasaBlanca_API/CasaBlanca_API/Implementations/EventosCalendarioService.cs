using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models.DTO.Eventos;
using CasaBlanca_API.Models.DTO.Rol;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CasaBlanca_API.Implementations
{
    public class EventosCalendarioService : IEventosCalendarioService
    {
        private readonly IConfiguration _configuration;

        public EventosCalendarioService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<ListaEventosResponse>> GetEventosCalendarioAsync(int year, int month)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                string query = @"SELECT eventosingreso.id,
                                   eventosingreso.idinmueble,
                                   eventosingreso.idestatusevento,
                                   estatusevento.estatusevento,
                                   eventosingreso.fechaevento,
                                   eventosingreso.recibonumero,
                                   eventosingreso.fechapago,
                                   eventosingreso.apartado,
                                   eventosingreso.liquida,
                                   eventosingreso.luz,
                                   eventosingreso.depositogarantia,
                                   eventosingreso.limpiezadomingo,
                                   eventosingreso.rentainmobiliario,
                                   eventosingreso.borrado,
                                   ubicacion.nombreubicacion,
                                   inmueble.numerocasa
                            FROM   eventosingreso
                                   INNER JOIN inmueble
                                           ON eventosingreso.idinmueble = inmueble.id
                                   INNER JOIN estatusevento
                                           ON eventosingreso.idestatusevento = estatusevento.id
                                   INNER JOIN ubicacion
                                           ON inmueble.idubicacion = ubicacion.id 
                            WHERE 
                                YEAR(EventosIngreso.FechaEvento) = @Anio
                            AND 
                                MONTH(EventosIngreso.FechaEvento) = @Mes
                            AND 
                                (eventosingreso.Borrado = 0)";

                /*
                string query = @"SELECT eventosingreso.id,
                                   eventosingreso.idinmueble,
                                   inmueble.numerocasa,
                                   inmueble.idubicacion,
                                   ubicacion.nombreubicacion,
                                   eventosingreso.idestatusevento,
                                   estatusevento.estatusevento,
                                   eventosingreso.fechaevento,
                                   eventosingreso.FechaPago,
                                   eventosingreso.recibonumero,
                                   usuario.nombre,
                                   usuario.apellidos,
                                   eventosingreso.apartado,
                                   eventosingreso.liquida,
                                   eventosingreso.luz,
                                   eventosingreso.depositogarantia,
                                   eventosingreso.limpiezadomingo,
                                   eventosingreso.rentainmobiliario,
                                   eventosingreso.comentarios
                            FROM   eventosingreso
                                   INNER JOIN inmueble
                                           ON eventosingreso.idinmueble = inmueble.id
                                   INNER JOIN ubicacion
                                           ON inmueble.idubicacion = ubicacion.id
                                   INNER JOIN estatusevento
                                           ON eventosingreso.idestatusevento = estatusevento.id
                                   LEFT OUTER JOIN casa_usuario
                                                ON inmueble.id = casa_usuario.idcasa
                                   LEFT OUTER JOIN usuario
                                                ON casa_usuario.idusuario = usuario.id
                                WHERE YEAR(EventosIngreso.FechaEvento) = @Anio 
                                  AND MONTH(EventosIngreso.FechaEvento) = @Mes
                                  AND (eventosingreso.Borrado = 0)
                //                ORDER BY EventosIngreso.FechaEvento ASC;";
                */

                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<ListaEventosResponse>(query, new { Anio = year, Mes = month });
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


    }
}
