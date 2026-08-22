using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models.DTO.ResumenAnalitico;
using CasaBlanca_API.Models.DTO.Rol;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CasaBlanca_API.Implementations
{
    public class EgresoAnaliticoService : IEgresoAnaliticoService
    {
        private readonly IConfiguration _configuration;

        public EgresoAnaliticoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<ResumenAnaliticoResponse>> GetResumenAnaliticoAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                string query = @"Exec sp_getTransaccionesRecientes";

                using (var connection = new SqlConnection(connectionString))
                {
                    IEnumerable<ResumenAnaliticoResponse> result = await connection.QueryAsync<ResumenAnaliticoResponse>(query);
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
