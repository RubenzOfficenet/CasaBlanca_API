using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models.DTO.Casas;
using CasaBlanca_API.Models.DTO.Eventos;
using CasaBlanca_API.Models.DTO.Rol;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Data;

namespace CasaBlanca_API.Implementations
{
    public class EventoService : IEventoService
    {
        private readonly IConfiguration _configuration;

        public EventoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<EventoResponse>> GetAllEventsAsync(EventoGetAllRequest eventoFiltro)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                string query = @"DECLARE @anio INTEGER = @year
                                    DECLARE @mes INTEGER = @month
                                    DECLARE @fechaInicio DATE = DATEFROMPARTS(@anio, @mes, 1)
                                    DECLARE @fechaFin DATE = DATEADD(MONTH, 1, @fechaInicio)

                                    SELECT eventosingreso.id, eventosingreso.idinmueble, eventosingreso.idestatusevento,
                                           inmueble.idubicacion, ubicacion.nombreubicacion, inmueble.numerocasa,
                                           eventosingreso.fechaevento, 
                                           eventosingreso.recibonumero, eventosingreso.fechapago, eventosingreso.apartado,
                                           eventosingreso.liquida, eventosingreso.luz, eventosingreso.depositogarantia,
                                           eventosingreso.limpiezadomingo, eventosingreso.rentainmobiliario,
                                           estatusevento.estatusevento,
                                           eventosingreso.apartado + eventosingreso.liquida + eventosingreso.luz + eventosingreso.depositogarantia + eventosingreso.limpiezadomingo + eventosingreso.rentainmobiliario AS Total,
                                           (SELECT COUNT(*) FROM EgresoEvento WHERE IdEventoIngreso = eventosingreso.id) AS TotalEgresos
                                    FROM   eventosingreso
                                           INNER JOIN inmueble ON eventosingreso.idinmueble = inmueble.id
                                           INNER JOIN estatusevento ON eventosingreso.idestatusevento = estatusevento.id
                                           INNER JOIN ubicacion ON inmueble.idubicacion = ubicacion.id
                                    WHERE  eventosingreso.fechaevento >= @fechaInicio
                                           AND eventosingreso.fechaevento < @fechaFin
                                           AND eventosingreso.Borrado = 0 
                                    ORDER BY eventosingreso.fechaevento DESC";
                /*
                //string query = @"SELECT eventosingreso.id,
                //                   eventosingreso.idinmueble,
                //                   eventosingreso.idestatusevento,
                //                   inmueble.idubicacion,
                //                   ubicacion.nombreubicacion,
                //                   inmueble.numerocasa,
                //                   eventosingreso.fechaevento,
                //                   inmueble.nombretitular,
                //                   inmueble.apellidostitular,
                //                   eventosingreso.recibonumero,
                //                   eventosingreso.fechapago,
                //                   eventosingreso.apartado,
                //                   eventosingreso.liquida,
                //                   eventosingreso.luz,
                //                   eventosingreso.depositogarantia,
                //                   eventosingreso.limpiezadomingo,
                //                   eventosingreso.rentainmobiliario,
                //                   estatusevento.estatusevento,
                //                   eventosingreso.apartado + eventosingreso.liquida + eventosingreso.luz + eventosingreso.depositogarantia + eventosingreso.limpiezadomingo + eventosingreso.rentainmobiliario as Total
                //            FROM   eventosingreso
                //                   INNER JOIN inmueble ON eventosingreso.idinmueble = inmueble.id
                //                   INNER JOIN estatusevento ON eventosingreso.idestatusevento = estatusevento.id
                //                   INNER JOIN ubicacion ON inmueble.idubicacion = ubicacion.id 
                //            WHERE  YEAR(eventosingreso.fechaevento) = @year
                //                   AND MONTH(eventosingreso.fechaevento) = @month
                //                   AND eventosingreso.Borrado = 0 ";
                */
                
                using (var connection = new SqlConnection(connectionString))
                {
                    IEnumerable<EventoResponse> result = await connection.QueryAsync<EventoResponse>(query, eventoFiltro);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> AddEventoAsync(EventoAddRequest eventoRequest)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");
                int result = 0;

                string query = @"INSERT INTO EventosIngreso
                                   (IdInmueble
                                   ,IdEstatusEvento
                                   ,FechaEvento
                                   ,ReciboNumero
                                   ,FechaPago
                                   ,Apartado
                                   ,Liquida
                                   ,Luz
                                   ,DepositoGarantia
                                   ,LimpiezaDomingo
                                   ,RentaInmobiliario
                                   ,DateAdded
                                   ,LastUpdate)
                             VALUES
                                   (@IdInmueble
                                   ,@IdEstatusEvento
                                   ,@FechaEvento
                                   ,@ReciboNumero
                                   ,@FechaPago
                                   ,@Apartado
                                   ,@Liquida
                                   ,@Luz
                                   ,@DepositoGarantia
                                   ,@LimpiezaDomingo
                                   ,@RentaInmobiliario
                                   ,GETDATE()
                                   ,null)";

                using (var connection = new SqlConnection(connectionString))
                {
                    result = await connection.ExecuteAsync(query, eventoRequest);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<EventoResponse> GetEventoingresosById(int idEvento)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                string query = @"SELECT eventosingreso.id,
                                   eventosingreso.idinmueble,
                                   eventosingreso.idestatusevento,
                                   inmueble.idubicacion,
                                   ubicacion.nombreubicacion,
                                   inmueble.numerocasa,
                                   eventosingreso.fechaevento,
                                   eventosingreso.recibonumero,
                                   eventosingreso.fechapago,
                                   eventosingreso.apartado,
                                   eventosingreso.liquida,
                                   eventosingreso.luz,
                                   eventosingreso.depositogarantia,
                                   eventosingreso.limpiezadomingo,
                                   eventosingreso.rentainmobiliario,
                                   estatusevento.estatusevento,
                                   eventosingreso.apartado + eventosingreso.liquida + eventosingreso.luz + eventosingreso.depositogarantia + eventosingreso.limpiezadomingo + eventosingreso.rentainmobiliario as Total
                            FROM   eventosingreso
                                   INNER JOIN inmueble ON eventosingreso.idinmueble = inmueble.id
                                   INNER JOIN estatusevento ON eventosingreso.idestatusevento = estatusevento.id
                                   INNER JOIN ubicacion ON inmueble.idubicacion = ubicacion.id 
                            WHERE  EventosIngreso.id = @idEvento";

                var parameters = new DynamicParameters();
                parameters.Add("@idEvento", idEvento, DbType.Int32);


                using (var connection = new SqlConnection(connectionString))
                {
                    EventoResponse result = await connection.QuerySingleOrDefaultAsync<EventoResponse>(query, parameters);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> UpdateEventoingreso(EventoIngresoUpdateRequest eventoIngresoUpdateRequest)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                string query = @"UPDATE EventosIngreso 
                                SET IdInmueble = @IdInmueble
                                   ,IdEstatusEvento = @IdEstatusEvento
                                   ,FechaEvento = @FechaEvento
                                   ,ReciboNumero = @ReciboNumero
                                   ,FechaPago = @FechaPago
                                   ,Apartado = @Apartado
                                   ,Liquida = @Liquida
                                   ,Luz = @Luz
                                   ,DepositoGarantia = @DepositoGarantia
                                   ,LimpiezaDomingo = @LimpiezaDomingo
                                   ,RentaInmobiliario = @RentaInmobiliario
                                   ,LastUpdate = GETDATE()
                                WHERE Id = @Id";

                using (var connection = new SqlConnection(connectionString))
                {
                    int rowsAffected = await connection.ExecuteAsync(query, eventoIngresoUpdateRequest);

                    return rowsAffected;                    
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el evento: {ex.Message}", ex);
            }
        }


        public async Task<int> DeleteEventoIngreso(int idEventoIngreso)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                string query = @"UPDATE EventosIngreso 
                                SET Borrado = 1
                                   ,LastUpdate = GETDATE()
                                WHERE Id = @idEventoIntreso";

                var parameters = new DynamicParameters();
                parameters.Add("@idEventoIntreso", idEventoIngreso, DbType.Int32);

                using (var connection = new SqlConnection(connectionString))
                {
                    int rowsAffected = await connection.ExecuteAsync(query, parameters);

                    return rowsAffected;
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al actualizar el evento: {ex.Message}", ex);
            }
        }


    }
}
