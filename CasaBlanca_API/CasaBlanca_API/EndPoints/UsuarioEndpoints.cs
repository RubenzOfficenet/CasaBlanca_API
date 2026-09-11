
using AutoMapper;
using CasaBlanca_API.Implementations;
using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models;
using CasaBlanca_API.Models.DTO;
using CasaBlanca_API.Models.DTO.Usuario;
using CasaBlanca_API.shared;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace CasaBlanca_API.EndPoints
{
    public static class UsuarioEndpoints
    {
        public static void ConfigureUsuarioEndpoints(this WebApplication app)
        {
            var connectionString = app.Configuration.GetConnectionString("DefultConnection");
            
            app.MapGet("/api/GetAllUsers", GetAllUsers).WithName("GetUsers").Produces<IEnumerable<GetUsuariosResponse>>(200).Produces(500);
            app.MapGet("/api/GetUserById/{id}", GetUserById).WithName("GetUserById").Produces<UsuarioResponse>(200).Produces(404).Produces(500);
            app.MapPost("/api/CreateUser", CreateUser).WithName("CreateUser").Produces<Usuario>(StatusCodes.Status201Created).Produces(StatusCodes.Status400BadRequest).Produces(StatusCodes.Status500InternalServerError);
            app.MapPut("/api/UpdateUsuario", UpdateUsuario).WithName("UpdateUsuario").Accepts<UsuarioEditRequest>("application/json").Produces<int>(200).Produces<string>(400).Produces<string>(500);
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

        public async static Task<IResult> GetUserById(int id, IMapper mapper, IUsuarioService usuarioService)
        {
            try
            {
                var user = await usuarioService.GetUsuarioByIdAsync(id);

                if (user == null)
                    return Results.NotFound($"User with id {id} not found.");

                return Results.Ok(user);
            }
            catch (Exception ex)
            {
                return Results.Problem($"An error occurred: {ex.Message}", statusCode: 500);
            }
        }

        public static async Task<IResult> CreateUser(UsuarioRequest usuarioRequest, IMapper mapper, IUsuarioService usuarioService)
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

                string textoAleatorio = TextGeneratorService.GenerateRandomTextChars();

                string passwordGenerated = PasswordHasher.GenerateFromSeed(textoAleatorio);

                usuarioRequest.Password = passwordGenerated;

                var result = usuarioService.AddUsuarioAsync(usuarioRequest);

                return Results.Ok(result.Id);

                // return Results.Created($"/api/Users/{usuario.Id}", usuario);    
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Error al crear el usuario");
            }
        }

        public async static Task<IResult> UpdateUsuario(UsuarioEditRequest usuarioUpdate, IUsuarioService usuarioService)
        {
            try
            {
                if (usuarioUpdate.Id <= 0)
                    return Results.BadRequest("Invalid user data Id is required.");

                // Optional: validate NumeroCasa
                if (usuarioUpdate.IdInmueble == 0)
                    return Results.BadRequest("Numero de Casa is required.");

                var rows = await usuarioService.ActualizarUsuarioAsync(usuarioUpdate);


                return TypedResults.Created($"/api/GetUserById/{usuarioUpdate.Id}", usuarioUpdate);

                //return Results.Created($"/api/users/{usuarioUpdate.Id}", new { Id = usuarioUpdate.Id });

                //return Results.Ok(rows);
            }
            catch (Exception ex)
            {
                return Results.InternalServerError($"An error occurred: {ex.Message}");
            }
        }


    }
}