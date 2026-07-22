using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models.DTO;
using CasaBlanca_API.Models.DTO.Casas;
using CasaBlanca_API.Models.DTO.Ingresos;
using Dapper;
using Microsoft.Data.SqlClient;

namespace CasaBlanca_API.Implementations
{
    public class IngresoService : IIngresoService
    {
        private readonly IConfiguration _configuration;

        public IngresoService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<int> AddIngresoAsync(IngresoRequest ingresoRequest)
        {
            try
             {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                string query = @"INSERT INTO Ingresos
                                   (IdCasa
                                   ,FechaRecepcion
                                   ,NumeroRecibo
                                   ,IdConcepto
                                   ,FechaConcepto
                                   ,Monto
                                   ,Observaciones)
                             VALUES
                                   (@IdCasa
                                   ,@FechaRecepcion
                                   ,@NumeroRecibo
                                   ,@IdConcepto
                                   ,@FechaConcepto
                                   ,@Monto
                                   ,@Observaciones)";

                using (var connection = new SqlConnection(connectionString))
                {
                    var result = await connection.ExecuteAsync(query, ingresoRequest);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<IEnumerable<IngresoResponse>> GetAllIngresosAsync()
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                string query = @"SELECT ingresos.id,
                                    inmueble.numerocasa,
                                    inmueble.nombretitular + ' ' +  inmueble.apellidostitular as nombretitular,
                                    inmueble.nombreocupante + ' ' + inmueble.apellidosocupante as nombreocupante,
                                    Inmueble.NumeroCasa,
                                    ingresos.fecharecepcion,
                                    ingresos.numerorecibo,
                                    concepto.concepto,
                                    ingresos.fechaconcepto,
                                    ingresos.monto,
                                    ingresos.observaciones
                                FROM   ingresos 
                                    INNER JOIN inmueble ON ingresos.idcasa = inmueble.id
                                    INNER JOIN concepto ON ingresos.idconcepto = concepto.id 
                                ORDER BY ingresos.fecharecepcion";

                using (var connection = new SqlConnection(connectionString))
                {
                    IEnumerable<IngresoResponse> result = await connection.QueryAsync<IngresoResponse>(query);
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
