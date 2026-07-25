using CasaBlanca_API.Interfaces;
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
                int result = 0;

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
                    result = await connection.ExecuteAsync(query, ingresoRequest);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<IEnumerable<IngresoResponse>> GetAllIngresosAsync(int year, int month)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                

                string query = @"SELECT ingresos.id,
                                       inmueble.numerocasa,
                                       inmueble.nombretitular + ' ' + inmueble.apellidostitular AS nombretitular,
                                       inmueble.nombreocupante + ' ' + inmueble.apellidosocupante AS nombreocupante,
                                       ingresos.fecharecepcion,
                                       ingresos.numerorecibo,
                                       concepto.concepto,
                                       ingresos.fechaconcepto,
                                       ingresos.monto,
                                       ingresos.observaciones,
                                       SUM(ingresos.monto) OVER () AS totalMonto
                                FROM ingresos
                                INNER JOIN inmueble ON ingresos.idcasa = inmueble.id
                                INNER JOIN concepto ON ingresos.idconcepto = concepto.id
                                WHERE (YEAR(ingresos.fecharecepcion) = @anio AND MONTH(ingresos.fecharecepcion) = @mes)
                                   OR (YEAR(ingresos.fechaconcepto) = @anio AND MONTH(ingresos.fechaconcepto) = @mes)
                                ORDER BY ingresos.fecharecepcion;";

                var parameters = new { anio = year, mes = month };

                using (var connection = new SqlConnection(connectionString))
                {
                    IEnumerable<IngresoResponse> result = await connection.QueryAsync<IngresoResponse>(query, parameters);
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
