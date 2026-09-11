using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models;
using CasaBlanca_API.Models.DTO;
using CasaBlanca_API.Models.DTO.Casas;
using CasaBlanca_API.Models.DTO.Ingresos;
using CasaBlanca_API.Models.DTO.Usuario;
using CasaBlanca_API.shared;
using Dapper;
using DocumentFormat.OpenXml.Bibliography;
using Microsoft.Data.SqlClient;

namespace CasaBlanca_API.Implementations
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IConfiguration _configuration;

        public UsuarioService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<GetUsuariosResponse>> GetUsuariosAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                string query = @"SELECT usuario.id,
                                   usuario.nombre,
                                   usuario.apellidos,
                                   usuario.email,
                                   usuario.celular,
                                   casa_usuario.idrol,
                                   roluser.rol,
                                   casa_usuario.idcasa,
                                   inmueble.numerocasa,
                                   inmueble.idubicacion,
                                   ubicacion.nombreubicacion,
                                   casa_usuario.idtiporelacion,
                                   tiporelacion.tiporelacion,
                                   usuario.idestatus,
                                   estatus.estatus
                            FROM   usuario
                                   INNER JOIN casa_usuario ON usuario.id = casa_usuario.idusuario
                                   INNER JOIN roluser ON casa_usuario.idrol = roluser.id
                                   INNER JOIN inmueble ON casa_usuario.idcasa = inmueble.id
                                   INNER JOIN ubicacion ON inmueble.idubicacion = ubicacion.id
                                   INNER JOIN tiporelacion ON casa_usuario.idtiporelacion = tiporelacion.id
                                   INNER JOIN estatus ON usuario.idestatus = estatus.id 
                            ORDER BY
                                inmueble.numerocasa;";

                using (var connection = new SqlConnection(connectionString))
                {
                    IEnumerable<GetUsuariosResponse> result = await connection.QueryAsync<GetUsuariosResponse>(query);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<IResult> AddUsuarioAsync(UsuarioRequest usuarioRequest)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    return Results.Problem(
                        detail: "La cadena de conexión 'DefaultConnection' no está configurada.",
                        statusCode: 500);
                }

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync(); // Es imprescindible abrir la conexión antes de iniciar la transacción

                    // 1. Validar si el correo ya existe (antes de abrir la transacción para evitar bloqueos innecesarios)
                    string sqlCheck = "SELECT COUNT(1) FROM Usuario WHERE email = @EmailUsuario";
                    int existe = await connection.ExecuteScalarAsync<int>(sqlCheck, new { EmailUsuario = usuarioRequest.EmailUsuario });

                    if (existe > 0)
                        return Results.BadRequest("Ya existe un usuario con ese correo electrónico.");

                    // Iniciamos la transacción explícita
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // 2. Insertar nuevo usuario
                            string queryUsuario = @"INSERT INTO Usuario
                                                   (Nombre
                                                   ,Apellidos
                                                   ,email
                                                   ,celular
                                                   ,password
                                                   ,IdEstatus)
                                             VALUES
                                                   (@Nombre
                                                   ,@Apellidos
                                                   ,@email
                                                   ,@celular
                                                   ,@password
                                                   ,@IdEstatus)
                                            SELECT CAST(SCOPE_IDENTITY() as int);";
                            
                            var parametersUsuario = new
                            {
                                Nombre = usuarioRequest.NombreUsuario,
                                Apellidos = usuarioRequest.ApellidosUsuario,
                                email = usuarioRequest.EmailUsuario,
                                celular = usuarioRequest.CelularUsuario,
                                password = PasswordHasher.GenerateFromSeed(usuarioRequest.Password),
                                IdEstatus = 1,
                                IdInmueble = usuarioRequest.IdInmueble,
                                IdRol = usuarioRequest.IdRol
                            };

                            // Se pasa el parámetro transaction a las llamadas de Dapper    
                            int newId = await connection.ExecuteScalarAsync<int>(queryUsuario, parametersUsuario, transaction);

                            // 3. Insertar relación Casa_Usuario
                            string queryCasaUsuario = @"INSERT INTO Casa_Usuario
                                                           (IdUsuario
                                                           ,IdRol
                                                           ,idCasa
                                                           ,IdTipoRelacion
                                                           ,DateAdded
                                                           ,LastUpdate)
                                                     VALUES
                                                           (@IdUsuario
                                                           ,@IdRol
                                                           ,@idCasa
                                                           ,@IdTipoRelacion
                                                           ,getdate()
                                                           ,null);";

                            var parametersCasaUsuario = new
                            {
                                IdUsuario = newId,
                                IdRol = usuarioRequest.IdRol,
                                idCasa = usuarioRequest.IdInmueble,
                                IdTipoRelacion = usuarioRequest.IdTipoRelacion
                            };

                            await connection.ExecuteAsync(queryCasaUsuario, parametersCasaUsuario, transaction);

                            // Confirmamos los cambios en la base de datos
                            transaction.Commit();

                            //return Results.Created($"/api/users/{newId}", new { Id = newId });
                            return Results.Ok();
                        }
                        catch
                        {
                            // Ante cualquier error durante las inserciones, revertimos los cambios
                            transaction.Rollback();
                            throw; // Re-lanzamos la excepción para que sea capturada por el catch principal
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message, statusCode: 500);
            }
        }

        public async Task<IResult> GetUsuarioByIdAsync(int Id)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                string query = @"SELECT usuario.id,
                                    usuario.nombre,
                                    usuario.apellidos,
                                    usuario.email,
                                    usuario.celular,
                                    casa_usuario.idrol,
                                    roluser.rol,
                                    casa_usuario.idcasa,
                                    inmueble.numerocasa,
                                    inmueble.id as 'IdInmueble',
                                    inmueble.idubicacion,
                                    ubicacion.nombreubicacion,
                                    casa_usuario.idtiporelacion,
                                    tiporelacion.tiporelacion,
                                    usuario.idestatus,
                                    estatus.estatus
                            FROM   usuario
                                    INNER JOIN casa_usuario ON usuario.id = casa_usuario.idusuario
                                    INNER JOIN roluser ON casa_usuario.idrol = roluser.id
                                    INNER JOIN inmueble ON casa_usuario.idcasa = inmueble.id
                                    INNER JOIN ubicacion ON inmueble.idubicacion = ubicacion.id
                                    INNER JOIN tiporelacion ON casa_usuario.idtiporelacion = tiporelacion.id
                                    INNER JOIN estatus ON usuario.idestatus = estatus.id 
                        WHERE usuario.id = @Id ";

                using (var connection = new SqlConnection(connectionString))
                {
                    var user = await connection.QueryFirstOrDefaultAsync<UsuarioResponseDTO>(query, new { Id });

                    if (user == null)
                        return Results.NotFound($"Usuario con id {Id} no encontrado.");

                    return Results.Ok(user);
                }
            }
            catch (Exception ex)
            {
                return Results.Problem($"Ocurrió un error: {ex.Message}", statusCode: 500);
            }
        }


        public async Task<IResult> ActualizarUsuarioAsync(UsuarioEditRequest usuario)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                if (string.IsNullOrEmpty(connectionString))
                {
                    return Results.Problem(
                        detail: "La cadena de conexión 'DefaultConnection' no está configurada.",
                        statusCode: 500);
                }

                using (var connection = new SqlConnection(connectionString))
                {
                    await connection.OpenAsync(); // Es imprescindible abrir la conexión antes de iniciar la transacción

                    // 1. Validar si el correo ya existe (antes de abrir la transacción para evitar bloqueos innecesarios)
                    string sqlCheck = "SELECT COUNT(1) FROM Usuario WHERE email = @EmailUsuario";
                    int existe = await connection.ExecuteScalarAsync<int>(sqlCheck, new { EmailUsuario = usuario.Email });

                    if (existe == 0)
                        return Results.BadRequest("E email del usuario no existe.");

                    // Iniciamos la transacción explícita
                    using (var transaction = connection.BeginTransaction())
                    {
                        try
                        {
                            // 2. Actualiza usuario
                            string queryUsuario = @"UPDATE Usuario
                                               SET Nombre = @Nombre
                                                  ,Apellidos = @Apellidos
                                                  ,email = @email
                                                  ,celular = @celular
                                                  ,LastUpdate = getdate()
                                             WHERE 
                                                Id = @id";

                            var parametersUsuario = new
                            {
                                id = usuario.Id,
                                nombre = usuario.Nombre,
                                Apellidos = usuario.Apellidos,
                                email = usuario.Email,
                                celular = usuario.Celular
                            };

                            // Se pasa el parámetro transaction a las llamadas de Dapper
                            int newId = await connection.ExecuteScalarAsync<int>(queryUsuario, parametersUsuario, transaction);

                            // 3. Actualiza relación Casa_Usuario
                            string queryCasaUsuario = @"UPDATE Casa_Usuario
                                                       SET IdRol = @IdRol
                                                          ,idCasa = @idCasa
                                                          ,IdTipoRelacion = @IdTipoRelacion
                                                          ,LastUpdate = getdate()
                                                     WHERE 
                                                        IdUsuario = @IdUsuario";

                            var parametersCasaUsuario = new
                            {
                                IdUsuario = usuario.Id,
                                IdRol = usuario.IdRol,
                                idcasa = usuario.IdInmueble,
                                IdTipoRelacion = usuario.IdTipoRelacion
                            };

                            await connection.ExecuteAsync(queryCasaUsuario, parametersCasaUsuario, transaction);

                            // Confirmamos los cambios en la base de datos
                            transaction.Commit();

                            return Results.Created($"/api/users/{usuario.Id}", new { Id = usuario.Id });
                        }
                        catch
                        {
                            // Ante cualquier error durante las inserciones, revertimos los cambios
                            transaction.Rollback();
                            throw; // Re-lanzamos la excepción para que sea capturada por el catch principal
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message, statusCode: 500);
            }
        }

    }
}
