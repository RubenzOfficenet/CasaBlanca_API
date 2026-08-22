using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models.DTO;
using CasaBlanca_API.Models.DTO.Catalogo;
using CasaBlanca_API.Models.DTO.Even_toEgreso;
using CasaBlanca_API.Models.DTO.Rol;
using CasaBlanca_API.Models.DTO.Ubicacion;
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

        public async Task<IEnumerable<UbicacionResponseDTO>> GetAllUbicaciones()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                string query = @"SELECT id, NombreUbicacion FROM ubicacion ORDER BY NombreUbicacion";

                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<UbicacionResponseDTO>(query);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<IEnumerable<EstatusEventoResponse>> GetAllEstatusEvento()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                string query = @"SELECT Id, EstatusEvento FROM EstatusEvento order by EstatusEvento";

                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<EstatusEventoResponse>(query);

                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }


        public async Task<IEnumerable<ConceptoEventoEgresoDTO>> GetCatalogoEventosEgreso()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                string query = @"SELECT Id, ConceptoEgreso FROM  ConceptoEgresoEvento ORDER BY ConceptoEgreso";

                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.QueryAsync<ConceptoEventoEgresoDTO>(query);

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
