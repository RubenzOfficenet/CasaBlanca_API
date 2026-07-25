using AutoMapper;
using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models.DTO.Casas;
using CasaBlanca_API.Models.DTO.Ingresos;

namespace CasaBlanca_API.EndPoints
{
    public static class IngresosEndPoints
    {
        public static void ConfigureIngresosEndPoints(this WebApplication app)
        {
            app.MapPost("/api/AddIngreso", AddIngreso).WithName("AddIngreso").Produces<int>(StatusCodes.Status200OK);
            app.MapGet("/api/GetIngresos", GetIngresos).WithName("GetIngresos").Produces<IEnumerable<IngresoResponse>>(StatusCodes.Status200OK).Produces(StatusCodes.Status500InternalServerError);
        }

        private static async Task<IResult> AddIngreso(IngresoRequest ingresoRequest, IIngresoService ingresoService, IMapper mapper)
        {
            try
            {
                int result = await ingresoService.AddIngresoAsync(ingresoRequest);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

        private static async Task<IResult> GetIngresos(IIngresoService ingresoService, int year, int month)
        {
            try
            {
                var result = await ingresoService .GetAllIngresosAsync(year, month);

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    title: "Error al obtener los ingresos",
                    statusCode: StatusCodes.Status500InternalServerError);
            }
        }




    }
}