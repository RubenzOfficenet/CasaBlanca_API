using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models.DTO;
using CasaBlanca_API.Models.DTO.Catalogo;
using CasaBlanca_API.Models.DTO.Rol;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CasaBlanca_API.Implementations
{
    public class CatalogosService : ICatalogosService
    {
        private readonly IConfiguration _configuration;

        public CatalogosService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<IEnumerable<RolDTO>> GetAllRolAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                string query = @"SELECT id, Rol FROM RolUser ORDER BY Rol";

                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<RolDTO>(query);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<IEnumerable<CatalogoIngresoResponse>> GetAllConceptoIngresosAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                string query = @"SELECT Id, concepto FROM Concepto WHERE tipoconcepto = 'ingresos' ORDER BY concepto";

                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<CatalogoIngresoResponse>(query);
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
