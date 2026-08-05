using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models.DTO;
using Dapper;
using Microsoft.Data.SqlClient;


namespace CasaBlanca_API.Implementations;

public  class InmuebleService : IInmuebleService
{
    private readonly IConfiguration _configuration;

    public InmuebleService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public async Task<int> UpdateInmuebleAsync(UpdateCasaDTO inmueble)
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefultConnection");

            string query = @"UPDATE [Inmueble]
                               SET [NumeroCasa] = @NumeroCasa
                                  ,[IdUbicacion] = @IdUbicacion
                                  ,[CuotaDeMantenimientoBase] = @CuotaDeMantenimientoBase
                                  ,[EstadoOcupacion] = @EstadoOcupacion
                                  ,[NombreTitular] = @NombreTitular
                                  ,[ApellidosTitular] = @ApellidosTitular
                                  ,[EmailTitular] = @EmailTitular
                                  ,[CelularTitular] = @CelularTitular
                                  ,[NombreOcupante] = @NombreOcupante
                                  ,[ApellidosOcupante] = @ApellidosOcupante
                                  ,[EmailOcupante] = @EmailOcupante
                                  ,[CelularOcupante] = @CelularOcupante
                                  ,[NumeroHabitantes] = @NumeroHabitantes
                                  ,[Observaciones] = @Observaciones
                                  ,[LastUpdated] = GETUTCDATE()
                             WHERE [Id] = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                var result = await connection.ExecuteAsync(query, inmueble);
                return result;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    public async Task<InmuebleReadDTO?> GetInmuebleByIdAsync(int id)
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefultConnection");

            string query = @"SELECT [Id]
                              ,[NumeroCasa]
                              ,[IdUbicacion]
                              ,[CuotaDeMantenimientoBase]
                              ,[EstadoOcupacion]
                              ,[NombreTitular]
                              ,[ApellidosTitular]
                              ,[EmailTitular]
                              ,[CelularTitular]
                              ,[NombreOcupante]
                              ,[ApellidosOcupante]
                              ,[EmailOcupante]
                              ,[CelularOcupante]
                              ,[NumeroHabitantes]
                              ,[Observaciones]
                              ,[DateAdded]
                              ,[LastUpdated]
                          FROM 
                              [Inmueble]
                           WHERE [Id] = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                var inmueble = await connection.QuerySingleOrDefaultAsync<InmuebleReadDTO>(query, new { Id = id });
                return inmueble;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    public async Task<int> CountByNumeroCasaAsync(string numeroCasa)
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefultConnection");

            string query = "SELECT COUNT(1) FROM [Inmueble] WHERE [NumeroCasa] = @NumeroCasa";

            using (var connection = new SqlConnection(connectionString))
            {
                var count = await connection.ExecuteScalarAsync<int>(query, new { NumeroCasa = numeroCasa });
                return count;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }
    }

    public async Task<int> CreateInmuebleAsync(InmuebleCreateDTO inmueble)
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefultConnection");

            string query = @"INSERT INTO Inmueble
                               (NumeroCasa
                               ,IdUbicacion
                               ,CuotaDeMantenimientoBase
                               ,EstadoOcupacion
                               ,NombreTitular
                               ,ApellidosTitular
                               ,EmailTitular
                               ,CelularTitular
                               ,NombreOcupante
                               ,ApellidosOcupante
                               ,EmailOcupante
                               ,CelularOcupante
                               ,NumeroHabitantes
                               ,Observaciones
                               ,Usuario
                               ,Password)
                         VALUES
                               (@NumeroCasa
                               ,@IdUbicacion
                               ,@CuotaDeMantenimientoBase
                               ,@EstadoOcupacion
                               ,@NombreTitular
                               ,@ApellidosTitular
                               ,@EmailTitular
                               ,@CelularTitular
                               ,@NombreOcupante
                               ,@ApellidosOcupante
                               ,@EmailOcupante
                               ,@CelularOcupante
                               ,@NumeroHabitantes
                               ,@Observaciones
                               ,@Usuario
                               ,@Password)";

            using (var connection = new SqlConnection(connectionString))
            {
                var result = await connection.ExecuteAsync(query, inmueble);
                return result;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }


    public async Task<IEnumerable<EstadoOcupacionReadDTO>> GetEstadosOcupacionAsync()
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefultConnection");

            string query = @"SELECT [Id], [EstadoInicialOcupacion] FROM [EstadoOcupacion]";

            using (var connection = new SqlConnection(connectionString))
            {
                IEnumerable<EstadoOcupacionReadDTO> estados = await connection.QueryAsync<EstadoOcupacionReadDTO>(query);
                return estados;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }


    public async Task<IEnumerable<ListaCasasDTO>> GetInmueblesAsync()
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefultConnection");

            string query = @"SELECT inmueble.id,
                                    inmueble.numerocasa,
                                    inmueble.idubicacion,
                                    ubicacion.nombreubicacion,
                                    inmueble.cuotademantenimientobase,
                                    inmueble.EstadoOcupacion,
                                    estadoocupacion.estadoinicialocupacion,
                                    inmueble.nombretitular,
                                    inmueble.apellidostitular,
                                    inmueble.emailtitular,
                                    inmueble.celulartitular,
                                    inmueble.nombreocupante,
                                    inmueble.apellidosocupante,
                                    inmueble.emailocupante,
                                    inmueble.celularocupante,
                                    inmueble.numerohabitantes,
                                    inmueble.observaciones
                            FROM   inmueble
                                   INNER JOIN ubicacion ON inmueble.idubicacion = ubicacion.id
                                   INNER JOIN estadoocupacion ON inmueble.estadoocupacion = estadoocupacion.id
                            ORDER  BY 
                                   inmueble.numerocasa ";

            using (var connection = new SqlConnection(connectionString))
            {
                IEnumerable<ListaCasasDTO> inmuebles = await connection.QueryAsync<ListaCasasDTO>(query);
                return inmuebles;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }

    


}
