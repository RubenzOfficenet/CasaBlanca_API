
using AutoMapper;
using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models;
using CasaBlanca_API.Models.DTO.Usuario;
using CasaBlanca_API.shared;
using Microsoft.EntityFrameworkCore;

namespace CasaBlanca_API.EndPoints
{
    public static class UsuarioEndpoints
    {
        public static void ConfigureUsuarioEndpoints(this WebApplication app)
        {
            var connectionString = app.Configuration.GetConnectionString("DefultConnection");
            
            app.MapGet("/api/Users", GetAllUsers).WithName("GetUsers").Produces<IEnumerable<Usuario>>(200).Produces(500);
            app.MapGet("/api/Users/{id}", GetUserById).WithName("GetUserById").Produces<Usuario>(200).Produces(404).Produces(500);
            app.MapPost("/api/NewUser", CreateUser).WithName("CreateUser").Produces<Usuario>(StatusCodes.Status201Created).Produces(StatusCodes.Status400BadRequest).Produces(StatusCodes.Status500InternalServerError);
        }

        public async static Task<IResult> GetAllUsers(IUsuarioService service)
        {
            try
            {
                IEnumerable<GetUsuariosResponse> users = await service.GetUsuariosAsync();
                return Results.Ok(users);
            }
            catch (Exception ex)
            {
                return Results.InternalServerError($"An error occurred: {ex.Message}");
            }
        }

        public async static Task<IResult> GetUserById(CasaBlancaDbContext db, int id)
        {
            try
            {
                var user = await db.Usuarios.AsNoTracking().FirstOrDefaultAsync(u => u.Id == id);
                if (user == null)
                    return Results.NotFound($"User with id {id} not found.");

                return Results.Ok(user);
            }
            catch (Exception ex)
            {
                return Results.InternalServerError($"An error occurred: {ex.Message}");
            }
        }

        static async Task<IResult> CreateUser(UsuarioRequest usuarioRequest, CasaBlancaDbContext db, IMapper mapper)
        {
            try
            {
                // Validaciones básicas
                if (string.IsNullOrWhiteSpace(usuarioRequest.NombreUsuario))
                    return Results.BadRequest("El nombre del usuario es obligatorio.");

                if (string.IsNullOrWhiteSpace(usuarioRequest.EmailUsuario))
                    return Results.BadRequest("El correo electrónico es obligatorio.");

                if (string.IsNullOrWhiteSpace(usuarioRequest.CelularUsuario))
                    return Results.BadRequest("El celular es obligatorio.");

                // Verificar si el correo ya existe
                bool existe = await db.Usuarios.AnyAsync(x => x.EmailUsuario == usuarioRequest.EmailUsuario);

                if (existe)
                    return Results.BadRequest("Ya existe un usuario con ese correo electrónico.");

                var usuariMaapper = mapper.Map<Usuario>(usuarioRequest);
                usuariMaapper.Password = PasswordHasher.HashPassword(usuariMaapper.Password);
                usuariMaapper.IdEstatus = 1; // Asignar un valor predeterminado para IdEstatus
                usuariMaapper.DateAdded = DateTime.Now;
                usuariMaapper.DateUpdated = null;

                db.Usuarios.Add(usuariMaapper);
                await db.SaveChangesAsync();

                return Results.Created($"/api/Users/{usuariMaapper.Id}", usuariMaapper);
                
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Error al crear el usuario");
            }
        }

    }
}