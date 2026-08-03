using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models.DTO.Casas;
using CasaBlanca_API.Models.DTO.Egresos;
using CasaBlanca_API.Models.DTO.Ingresos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CasaBlanca_API.Implementations;

public class EgresosService : IEgresoService
{
    private readonly IConfiguration _configuration;

    public EgresosService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<IEnumerable<EgresoResponse>> GetAllEgresosAsync(int year, int month, CancellationToken cancellationToken = default)
    {
        const string query = @"
                            SELECT Id
                                  ,FechaEgreso
                                  ,Beneficiario
                                  ,Concepto
                                  ,Monto
                                  ,Observaciones
                                  ,SUM(monto) OVER () AS totalMonto
                            FROM dbo.Egresos
                            WHERE YEAR(FechaEgreso) = @Anio
                              AND MONTH(FechaEgreso) = @Mes
                              AND Borrado = 0
                            ORDER BY FechaEgreso;";

        try
        {
            var connectionString = _configuration.GetConnectionString("DefultConnection");

            await using var connection = new SqlConnection(connectionString);

            var command = new CommandDefinition(
                query,
                new { Anio = year, Mes = month },
                cancellationToken: cancellationToken);

            var result = await connection.QueryAsync<EgresoResponse>(command);

            return result;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<int> AddEgresoAsync(EgresoRequest egresoRequest, CancellationToken cancellationToken = default)
    {
        const string query = @"
                            INSERT INTO Egresos
                                   (FechaEgreso
                                   ,Beneficiario
                                   ,Concepto
                                   ,Monto
                                   ,Observaciones
                                   ,Borrado
                                   ,DateAdded
                                   ,LastUpdate)
                            VALUES
                                   (@FechaEgreso
                                   ,@Beneficiario
                                   ,@Concepto
                                   ,@Monto
                                   ,@Observaciones
                                   ,0
                                   ,GETDATE()
                                   ,NULL);
                            SELECT CAST(SCOPE_IDENTITY() AS int);";

        try
        {
            var connectionString = _configuration.GetConnectionString("DefultConnection");

            await using var connection = new SqlConnection(connectionString);

            var command = new CommandDefinition(
                query,
                new
                {
                    egresoRequest.FechaEgreso,
                    egresoRequest.Beneficiario,
                    egresoRequest.Concepto,
                    egresoRequest.Monto,
                    egresoRequest.Observaciones
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

    public async Task<EgresoResponse?> GetEgresoByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        const string query = @"
                        SELECT Id
                              ,FechaEgreso
                              ,Beneficiario
                              ,Concepto
                              ,Monto
                              ,Observaciones
                        FROM dbo.Egresos
                        WHERE Id = @Id
                          AND Borrado = 0;";

        try
        {
            var connectionString = _configuration.GetConnectionString("DefultConnection");

            await using var connection = new SqlConnection(connectionString);

            var command = new CommandDefinition(
                query,
                new { Id = id },
                cancellationToken: cancellationToken);

            var result = await connection.QueryFirstOrDefaultAsync<EgresoResponse>(command);

            return result;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<int> UpdateEgresoAsync(int id, EgresoRequest egresoRequest, CancellationToken cancellationToken = default)
    {
        const string query = @"
                        UPDATE Egresos
                        SET FechaEgreso = @FechaEgreso
                           ,Beneficiario = @Beneficiario
                           ,Concepto = @Concepto
                           ,Monto = @Monto
                           ,Observaciones = @Observaciones
                           ,LastUpdate = GETDATE()
                        WHERE Id = @Id
                          AND Borrado = 0;";

        try
        {
            var connectionString = _configuration.GetConnectionString("DefultConnection");

            await using var connection = new SqlConnection(connectionString);

            var command = new CommandDefinition(
                query,
                new
                {
                    Id = id,
                    egresoRequest.FechaEgreso,
                    egresoRequest.Beneficiario,
                    egresoRequest.Concepto,
                    egresoRequest.Monto,
                    egresoRequest.Observaciones
                },
                cancellationToken: cancellationToken);

            int rowsAffected = await connection.ExecuteAsync(command);

            return rowsAffected;
        }
        catch (Exception ex)
        {
            throw;
        }
    }

    public async Task<int> DeleteEgresoAsync(int id, CancellationToken cancellationToken = default)
    {
        const string query = @"
                    UPDATE Egresos
                    SET Borrado = 1
                       ,LastUpdate = GETDATE()
                    WHERE Id = @Id
                      AND Borrado = 0;";

        try
        {
            var connectionString = _configuration.GetConnectionString("DefultConnection");

            await using var connection = new SqlConnection(connectionString);

            var command = new CommandDefinition(
                query,
                new { Id = id },
                cancellationToken: cancellationToken);

            int rowsAffected = await connection.ExecuteAsync(command);

            return rowsAffected;
        }
        catch (Exception ex)
        {
            throw;
        }
    }


}
