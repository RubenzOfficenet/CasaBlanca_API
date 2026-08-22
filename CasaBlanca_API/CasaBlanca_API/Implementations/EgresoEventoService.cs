using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models;
using CasaBlanca_API.Models.DTO.Egresos;
using CasaBlanca_API.Models.DTO.EventoEgreso;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CasaBlanca_API.Implementations
{
    public class EgresoEventoService : IEgresoEventoService
    {
        private readonly IConfiguration _configuration;

        public EgresoEventoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> AddEgresoEventoAsync(EgresoEventoDTO egresoEventoRequest, CancellationToken cancellationToken = default)
        {
            const string query = @"INSERT INTO [dbo].[EgresoEvento]
                                   ([IdEventoIngreso]
                                   ,[IdConceptoEgresoEvento]
                                   ,[MontoEgreso]
                                   ,[FechaEgreso]
                                   ,[Observaciones]
                                   ,[AddDate]
                                   ,[LastUpdated])
                             VALUES
                                   (@IdEventoIngreso
                                   ,@IdConceptoEgresoEvento
                                   ,@MontoEgreso
                                   ,@FechaEgreso
                                   ,@Observaciones
                                   ,GETDATE()
                                   ,null);
                            SELECT CAST(SCOPE_IDENTITY() AS int);";

            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                await using var connection = new SqlConnection(connectionString);

                var command = new CommandDefinition(
                    query,
                    new
                    {
                        egresoEventoRequest.IdEventoIngreso,
                        egresoEventoRequest.IdConceptoEgresoEvento,
                        egresoEventoRequest.MontoEgreso,
                        egresoEventoRequest.FechaEgreso,
                        egresoEventoRequest.Observaciones
                    },
                    cancellationToken: cancellationToken);

                int newId = await connection.ExecuteScalarAsync<int>(command);

                return newId;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<EgresoEventoResponse>> GetListaEgresoEventoAsync(int idEventoEgreso, CancellationToken cancellationToken = default)
        {
            const string query = @"SELECT egresoevento.id,
                                   egresoevento.ideventoingreso,
                                   inmueble.idubicacion,
                                   eventosingreso.fechaevento,
                                   ubicacion.nombreubicacion,
                                   inmueble.numerocasa,
                                   conceptoegresoevento.conceptoegreso,
                                   egresoevento.montoegreso,
                                   egresoevento.fechaegreso,
                                   egresoevento.observaciones
                            FROM   egresoevento
                                   INNER JOIN eventosingreso ON egresoevento.ideventoingreso = eventosingreso.id
                                   INNER JOIN inmueble ON eventosingreso.idinmueble = inmueble.id
                                   INNER JOIN ubicacion ON inmueble.idubicacion = ubicacion.id
                                   INNER JOIN conceptoegresoevento ON egresoevento.idconceptoegresoevento = conceptoegresoevento.id
                            WHERE  egresoevento.ideventoingreso = @IdEventoEgreso";

            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                await using var connection = new SqlConnection(connectionString);

                var command = new CommandDefinition(
                    query,
                    new { IdEventoEgreso = idEventoEgreso },
                    cancellationToken: cancellationToken);

                IEnumerable<EgresoEventoResponse> listaEgresos = await connection.QueryAsync<EgresoEventoResponse>(command);

                return listaEgresos;
            }
            catch (Exception)
            {
                throw;
            }
        }



    }
}
