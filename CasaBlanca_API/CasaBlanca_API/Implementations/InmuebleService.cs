using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models.DTO;
using CasaBlanca_API.Models.DTO.Casa;
using Dapper;
using Microsoft.Data.SqlClient;
using System.Linq.Expressions;


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

            string query = @"UPDATE Inmueble
                           SET NumeroCasa = @NumeroCasa
                              ,IdUbicacion = @IdUbicacion
                              ,CuotaDeMantenimientoBase = @CuotaDeMantenimientoBase
                              ,IdEstadoOcupacion = @IdEstadoOcupacion
                              ,NumeroHabitantes = @NumeroHabitantes
                              ,Observaciones = @Observaciones
                              ,LastUpdated = GetDate()
                         WHERE id = @Id;";

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

            string query = @"SELECT 
                            Id,
                      	    NumeroCasa, 
	                        IdUbicacion, 
	                        CuotaDeMantenimientoBase, 
	                        IdEstadoOcupacion, 
	                        NumeroHabitantes, 
	                        Observaciones
                          FROM 
                              [Inmueble]
                           WHERE [Id] = @Id";

            using (var connection = new SqlConnection(connectionString))
            {
                InmuebleReadDTO inmueble = new InmuebleReadDTO();
                inmueble = await connection.QuerySingleOrDefaultAsync<InmuebleReadDTO>(query, new { Id = id });
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
                               ,IdEstadoOcupacion
                               ,NumeroHabitantes
                               ,Observaciones
                               ,DateAdded
                               ,LastUpdated)
                         VALUES
                               (@NumeroCasa
                               ,@IdUbicacion
                               ,@CuotaDeMantenimientoBase
                               ,@IdEstadoOcupacion
                               ,@NumeroHabitantes
                               ,@Observaciones
                               ,GetDATE()
                               ,null);";

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
                               inmueble.idestadoocupacion,
                               estadoocupacion.estadoinicialocupacion,
                               inmueble.numerohabitantes,
                               inmueble.observaciones,
                               casa_usuario.idusuario,
                               usuario.nombre,
                               usuario.apellidos,
                               casa_usuario.idcasa,
                               roluser.rol,
                               casa_usuario.idrol
                        FROM   inmueble
                               INNER JOIN ubicacion ON inmueble.idubicacion = ubicacion.id
                               INNER JOIN estadoocupacion ON inmueble.idestadoocupacion = estadoocupacion.id
                               LEFT JOIN casa_usuario ON inmueble.id = casa_usuario.idcasa
                               LEFT JOIN usuario ON casa_usuario.idusuario = usuario.id
                               LEFT JOIN roluser ON casa_usuario.idtiporelacion = roluser.id 
                        ORDER BY
                            ubicacion.nombreubicacion";

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

    public async Task<IEnumerable<CasaDto>> GetInmuebleByIdUbicacionAsync(int idubicacion)
    {
        try
        {
            var connectionString = _configuration.GetConnectionString("DefultConnection");

            string query = @"SELECT id,
                               numerocasa,
                               idubicacion,
                               nombretitular + ' ' + apellidostitular as nombretitular,
                               nombreocupante + ' ' + apellidosocupante as nombreocupante
                        FROM   inmueble
                        WHERE  ( idubicacion = @idubicacion) ";

            using (var connection = new SqlConnection(connectionString))
            {
                var inmueble = await connection.QueryAsync<CasaDto>(query, new { idUbicacion = idubicacion });
                return inmueble;
            }
        }
        catch (Exception ex)
        {
            throw new Exception(ex.Message);
        }

    }


}
