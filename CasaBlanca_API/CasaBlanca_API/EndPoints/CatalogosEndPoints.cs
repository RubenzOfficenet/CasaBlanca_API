using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models;
using CasaBlanca_API.Models.DTO.Rol;
using CasaBlanca_API.Models.DTO.Usuario;

namespace CasaBlanca_API.EndPoints
{
    public static class CatalogosEndPoints
    {
        public static void ConfigureCatalogosEndpoints(this WebApplication app)
        {
            var connectionString = app.Configuration.GetConnectionString("DefultConnection");

            app.MapGet("/api/GelAllRol", GetAllRol).WithName("GetRol").Produces<IEnumerable<RolDTO>>(200).Produces(500);
        }

        public async static Task<IResult> GetAllRol(ICatalogosService service)
        {
            try
            {
                IEnumerable<RolDTO> result = await service.GetAllRolAsync();
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.InternalServerError($"An error occurred: {ex.Message}");
            }
        }


    }
}
