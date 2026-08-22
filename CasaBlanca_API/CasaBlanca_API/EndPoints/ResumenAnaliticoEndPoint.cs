using AutoMapper;
using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models.DTO.Egresos;
using CasaBlanca_API.Models.DTO.ResumenAnalitico;

namespace CasaBlanca_API.EndPoints
{
    public static class ResumenAnaliticoEndPoint
    {
        public static void ConfigureResumenAnaliticoEndPoint(this WebApplication app)
        {
            app.MapGet("/api/GetRersumenAnalitico", GetRersumenAnalitico).WithName("GetRersumenAnalitico").Produces<int>(StatusCodes.Status200OK);
        }

        private static async Task<IResult> GetRersumenAnalitico(IEgresoAnaliticoService resumenAnaliticoService, IMapper mapper)
        {
            try
            {
                IEnumerable<ResumenAnaliticoResponse> result = await resumenAnaliticoService.GetResumenAnaliticoAsync();

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.Problem(ex.Message);
            }
        }

    }
}
