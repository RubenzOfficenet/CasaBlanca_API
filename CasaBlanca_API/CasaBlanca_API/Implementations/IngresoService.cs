using Azure.Core;
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
                                WHERE (
                                        (YEAR(ingresos.fecharecepcion) = @anio AND MONTH(ingresos.fecharecepcion) = @mes)
                                        OR (YEAR(ingresos.fechaconcepto) = @anio AND MONTH(ingresos.fechaconcepto) = @mes)
                                      )
                                  AND ingresos.borrado = 0
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

        public async Task<int> ActualizarIngresoAsync(IngresoUpdateRequest ingreso)
        {
            var connectionString = _configuration.GetConnectionString("DefultConnection");

            const string sql = @"UPDATE [dbo].[Ingresos]
                                   SET [IdCasa] = @IdCasa,
                                       [FechaRecepcion] = @FechaRecepcion,
                                       [NumeroRecibo] = @NumeroRecibo,
                                       [IdConcepto] = @IdConcepto,
                                       [FechaConcepto] = @FechaConcepto,
                                       [Monto] = @Monto,
                                       [Observaciones] = @Observaciones,
                                       [DateUpdated] = GETDATE()
                                 WHERE id = @Id";

            using var connection = new SqlConnection(connectionString);

            var parametros = new
            {
                ingreso.Id,
                ingreso.IdCasa,
                ingreso.FechaRecepcion,
                ingreso.NumeroRecibo,
                ingreso.IdConcepto,
                ingreso.FechaConcepto,
                ingreso.Monto,
                ingreso.Observaciones,
            };

            var filasAfectadas = await connection.ExecuteAsync(sql, parametros);
            return filasAfectadas;
        }

        public async Task<IngresoUpdateDTO?> GetIngresoByIdAsync(int id)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                const string query = @"SELECT ingresos.id,
                            ingresos.IdCasa,
                            inmueble.numerocasa,
                            ingresos.fecharecepcion,
                            ingresos.numerorecibo,
                            concepto.id as IdConcepto,
                            concepto.concepto,
                            ingresos.fechaconcepto,
                            ingresos.monto,
                            ingresos.observaciones
                    FROM ingresos
                    INNER JOIN inmueble ON ingresos.idcasa = inmueble.id
                    INNER JOIN concepto ON ingresos.idconcepto = concepto.id
                    WHERE ingresos.id = @Id";

                using var connection = new SqlConnection(connectionString);

                return await connection.QueryFirstOrDefaultAsync<IngresoUpdateDTO>(query, new { Id = id });
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> UpdateIngresoAsync(IngresoRequestDTO ingresoRequest)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                string query = @"UPDATE Ingresos
                                   SET IdCasa = @IdCasa
                                      ,FechaRecepcion = @FechaRecepcion
                                      ,NumeroRecibo = @NumeroRecibo
                                      ,IdConcepto = @IdConcepto
                                      ,FechaConcepto = @FechaConcepto
                                      ,Monto = @Monto
                                      ,Observaciones = @Observaciones
                                      ,DateUpdated = GETDATE()
                                 WHERE id = @id";

                var parameters = new DynamicParameters();
                parameters.Add("@Id", ingresoRequest.Id);
                parameters.Add("@IdCasa", ingresoRequest.IdCasa);
                parameters.Add("@FechaRecepcion", ingresoRequest.FechaRecepcion);
                parameters.Add("@NumeroRecibo", ingresoRequest.NumeroRecibo);
                parameters.Add("@IdConcepto", ingresoRequest.IdConcepto);
                parameters.Add("@FechaConcepto", ingresoRequest.FechaConcepto);
                parameters.Add("@Monto", ingresoRequest.Monto);
                parameters.Add("@Observaciones", ingresoRequest.Observaciones);

                using (var connection = new SqlConnection(connectionString))
                {
                    int result = await connection.ExecuteAsync(query, parameters);
                    return result;
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<int> DeleteIngresoAsync(int Id)
        {
            try
            {
                var connectionString = _configuration.GetConnectionString("DefultConnection");

                string query = @"UPDATE Ingresos
                                   SET Borrado = 1
                                      ,DateUpdated = GETDATE()
                                 WHERE id = @id";

                var parameters = new DynamicParameters();
                parameters.Add("@Id", Id);
               

                using (var connection = new SqlConnection(connectionString))
                {
                    int result = await connection.ExecuteAsync(query, parameters);
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
