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

                string query = @"SELECT 
                                    EventosIngreso.Id, 
                                    EventosIngreso.IdInmueble, 
                                    Inmueble.NumeroCasa, 
                                    Inmueble.IdUbicacion, 
                                    Ubicacion.NombreUbicacion, 
                                    EventosIngreso.IdEstatusEvento, 
                                    EstatusEvento.EstatusEvento, 
                                    EventosIngreso.FechaEvento, 
                                    EventosIngreso.ReciboNumero, 
                                    Usuario.Nombre, 
                                    Usuario.Apellidos,
                                    EventosIngreso.Apartado,
                                    EventosIngreso.Liquida  
                                FROM EventosIngreso
                                INNER JOIN Inmueble 
                                    ON EventosIngreso.IdInmueble = Inmueble.Id
                                INNER JOIN Ubicacion 
                                    ON Inmueble.IdUbicacion = Ubicacion.Id
                                INNER JOIN EstatusEvento 
                                    ON EventosIngreso.IdEstatusEvento = EstatusEvento.Id
                                LEFT JOIN Casa_Usuario 
                                    ON Inmueble.Id = Casa_Usuario.idCasa
                                LEFT JOIN Usuario 
                                    ON Casa_Usuario.IdUsuario = Usuario.Id
                                WHERE YEAR(EventosIngreso.FechaEvento) = @Anio 
                                  AND MONTH(EventosIngreso.FechaEvento) = @Mes
                                ORDER BY EventosIngreso.FechaEvento ASC;";

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
