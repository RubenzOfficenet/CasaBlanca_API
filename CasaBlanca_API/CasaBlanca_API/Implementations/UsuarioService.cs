using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models.DTO;
using CasaBlanca_API.Models.DTO.Usuario;
using Dapper;
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

                string query = @"SELECT u.id AS IdUsuario,
                                   u.nombreusuario,
                                   u.apellidosusuario,
                                   u.emailusuario,
                                   u.celularusuario,
                                   u.idrol,
                                   r.rol,
                                   u.idinmueble,
                                   i.numerocasa,
                                   u.idestatus,
                                   e.estatus
                            FROM   usuario AS u
                                   INNER JOIN roluser AS r
                                           ON u.idrol = r.id
                                   INNER JOIN inmueble AS i
                                           ON u.idinmueble = i.id
                                   INNER JOIN estatus AS e
                                           ON u.idestatus = e.id
                            ORDER  BY u.nombreusuario ";

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
    }
}
