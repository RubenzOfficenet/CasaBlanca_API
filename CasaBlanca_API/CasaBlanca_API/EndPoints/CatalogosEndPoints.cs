using AutoMapper;
using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models;
using CasaBlanca_API.Models.DTO;
using CasaBlanca_API.Models.DTO.Casas;
using CasaBlanca_API.Models.DTO.Catalogo;
using CasaBlanca_API.Models.DTO.Even_toEgreso;
using CasaBlanca_API.Models.DTO.Rol;
using CasaBlanca_API.Models.DTO.Ubicacion;
using CasaBlanca_API.Models.DTO.Usuario;

namespace CasaBlanca_API.EndPoints
{
    public static class CatalogosEndPoints
    {
        public static void ConfigureCatalogosEndpoints(this WebApplication app)
        {
            var connectionString = app.Configuration.GetConnectionString("DefultConnection");

            app.MapGet("/api/GelAllRol", GetAllRol).WithName("GetRol").Produces<IEnumerable<RolDTO>>(200).Produces(500);
            app.MapGet("/api/GetCasas", GetAllCasas).WithName("GetAllCasas").Produces<IEnumerable<CasaResponse>>(200);
            app.MapGet("/api/GetEstatusEvento", GetEstatusEvento).WithName("GetEstatusEvento").Produces<IEnumerable<CasaResponse>>(200);
            app.MapGet("/api/GetConceptoIngreso", GetConceptoIngreso).WithName("GetConceptoIngreso").Produces<IEnumerable<CatalogoIngresoResponse>>(200);
            app.MapGet("/api/GetAllUbicaciones", GetAllUbicaciones).WithName("GetAllUbicaciones").Produces<IEnumerable<UbicacionResponseDTO>>(200);
            app.MapGet("/api/GetConceptoEventoEgreso", GetConceptoEventoEgreso).WithName("GetConceptoEventoEgreso").Produces<IEnumerable<ConceptoEventoEgresoDTO>>(200).Produces(500);
        }

        public async static Task<IResult> GetAllUbicaciones(ICatalogosService service)
        {
            try
            {
                IEnumerable<UbicacionResponseDTO> result = await service.GetAllUbicaciones();
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.InternalServerError($"An error occurred: {ex.Message}");
            }
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

        public async static Task<IResult> GetAllCasas(IInmuebleService inmuebleService, IMapper mapper)
        {
            try
            {
                IEnumerable<ListaCasasDTO> datos = await inmuebleService.GetInmueblesAsync();

                var responseList = mapper.Map<List<CasaResponse>>(datos);

                return Results.Ok(responseList);
            }
            catch (Exception ex)
            {
                return Results.InternalServerError($"An error occurred: {ex.Message}");
            }
        }

        public async static Task<IResult> GetConceptoIngreso(ICatalogosService ctcatalogoService, IMapper mapper)
        {
            try
            {
                IEnumerable<CatalogoIngresoResponse> result = await ctcatalogoService.GetAllConceptoIngresosAsync();

                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.InternalServerError($"An error occurred: {ex.Message}");
            }
        }

        public async static Task<IResult> GetEstatusEvento(ICatalogosService catalogoService)
        {
            try
            {
                IEnumerable<EstatusEventoResponse> datos = await catalogoService.GetAllEstatusEvento();

                return Results.Ok(datos);
            }
            catch (Exception ex)
            {
                return Results.InternalServerError($"An error occurred: {ex.Message}");
            }
        }

        public async static Task<IResult> GetConceptoEventoEgreso(ICatalogosService service)
        {
            try
            {
                IEnumerable<ConceptoEventoEgresoDTO> result = await service.GetCatalogoEventosEgreso();
                return Results.Ok(result);
            }
            catch (Exception ex)
            {
                return Results.InternalServerError($"An error occurred: {ex.Message}");
            }
        }
    }
}
