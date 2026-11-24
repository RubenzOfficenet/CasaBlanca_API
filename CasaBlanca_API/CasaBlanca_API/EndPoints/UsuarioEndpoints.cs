using CasaBlanca_API.Interfaces;
using CasaBlanca_API.Models.DTO.Usuario;
using CasaBlanca_API.shared;
using Microsoft.EntityFrameworkCore;
using Core.Mappy.Interfaces;

namespace CasaBlanca_API.EndPoints
{
    public static class UsuarioEndpoints
    {
        public static void ConfigureUsuarioEndpoints(this WebApplication app)
        {
            var connectionString = app.Configuration.GetConnectionString("DefultConnection");
            string? pswdEmail = app.Configuration["pswdEmail"];

            app.MapGet("/api/GetAllUsers", GetAllUsers).WithName("GetUsers").Produces<IEnumerable<GetUsuariosResponse>>(200).Produces(500);
            app.MapGet("/api/GetUserById/{id}", GetUserById).WithName("GetUserById").Produces<UsuarioResponse>(200).Produces(404).Produces(500);
            app.MapPost("/api/CreateUser", CreateUser).WithName("CreateUser").Produces(StatusCodes.Status200OK).Produces(StatusCodes.Status400BadRequest).Produces(StatusCodes.Status500InternalServerError);
            app.MapPut("/api/UpdateUsuario", UpdateUsuario).WithName("UpdateUsuario").Accepts<UsuarioEditRequest>("application/json").Produces<int>(200).Produces<string>(400).Produces<string>(500);
            app.MapGet("/api/GetUserByEmailAndPassword", GetUserByEmailAndPassword).WithName("GetUserByEmailAndPassword").Produces<LoginResponseDTO>(200).Produces(404).Produces(500);
            app.MapPut("/api/UpdatePassword", UpdatePassword).WithName("UpdatePassword").Accepts<UsuarioUpdatePasswordRuquest>("application/json").Produces<int>(200).Produces<string>(400).Produces<string>(500);
            app.MapPost("/api/RecoveryPassword", RecoveryPassword).WithName("RecoveryPassword").Produces(StatusCodes.Status200OK).Produces(500);

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

        public async static Task<IResult> GetUserByEmailAndPassword(string email, string password, IMapper mapper, IUsuarioService usuarioService)
        {
            try
            {
                string passwoordEnc = PasswordHasher.GenerateFromSeed(password);

                LoginResponseDTO? result = await usuarioService.GetUserByEmailAndPasswordAsync(email, passwoordEnc);

                return Results.Ok(result);
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


                // Add new user
                IResult result = await usuarioService.AddUsuarioAsync(usuarioRequest);

                // valida si hay un badrequest
                if (result is IStatusCodeHttpResult { StatusCode: 400 } && result is IValueHttpResult valueResult)
                {
                    return Results.Problem(
                     detail: "Datos incorrectos y duplicados.",
                     statusCode: StatusCodes.Status400BadRequest);
                }


                // send email
                
                var emailCore = new EmailCore();

                await emailCore.SendEmail(textoAleatorio, usuarioRequest.EmailUsuario, "Alta de nuevo usuario");


                return Results.Problem(
                     detail: "El usuario se agregó correctamente.",
                     statusCode: StatusCodes.Status200OK);

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

        public async static Task<IResult> UpdatePassword(UsuarioUpdatePasswordRuquest usuarioUpdatePswd, IUsuarioService usuarioService)
        {
            try
            {
                if (usuarioUpdatePswd.Id <= 0)
                    return Results.BadRequest("Invalid user data Id is required.");

                if (String.IsNullOrEmpty(usuarioUpdatePswd.Password))
                    return Results.BadRequest("Invalid user password is required.");
                    
                var rows = await usuarioService.ActualizarUsuarioPasswordAsync(usuarioUpdatePswd);


                return Results.Problem(
                                detail: "La contraseña se actualizó correctamente",
                                statusCode: 200);
                
            }
            catch (Exception ex)
            {
                return Results.InternalServerError($"An error occurred: {ex.Message}");
            }
        }

        public static async Task<IResult> RecoveryPassword(string email, IUsuarioService usuarioService)
        {
            try
            {
                // 1. Obtener el usuario primero (antes de generar contraseñas)
                var result = await usuarioService.GetUsuarioByEmailAsync(email);

                // Validar de forma segura si el resultado es Ok<UsuarioResponseDTO> y no es nulo
                if (result is not Microsoft.AspNetCore.Http.HttpResults.Ok<UsuarioResponseDTO> okResult || okResult.Value is null)
                {
                    return Results.NotFound(new { message = "No se encontró un usuario registrado con ese correo." });
                }

                UsuarioResponseDTO usuarioResponseDTO = okResult.Value;

                // 2. Generar la contraseña aleatoria y su hash
                string textoAleatorio = TextGeneratorService.GenerateRandomTextChars();
                string passwordGenerated = PasswordHasher.GenerateFromSeed(textoAleatorio);

                var usuarioUpdatePasswordRequest = new UsuarioUpdatePasswordRuquest
                {
                    Id = usuarioResponseDTO.Id,
                    Password = passwordGenerated
                };

                // 3. Actualizar el password en BD (faltaba el 'await')
                bool actualizado = await usuarioService.ResetPasswordUsuarioAsync(usuarioUpdatePasswordRequest);

                if (!actualizado)
                {
                    return Results.Problem(
                        detail: "No se pudo actualizar la contraseña en la base de datos.",
                        statusCode: StatusCodes.Status500InternalServerError,
                        title: "Error al actualizar");
                }

                // 4. Enviar el correo con la contraseña en texto plano
                var emailCore = new EmailCore();
                await emailCore.SendEmail(textoAleatorio, email, "Recuperar contraseña");

                // 5. Devolver Results.Ok en lugar de Results.Problem con código 200
                return Results.Ok(new { message = "Se recuperó la contraseña correctamente." });
            }
            catch (Exception ex)
            {
                return Results.Problem(
                    detail: ex.Message,
                    statusCode: StatusCodes.Status500InternalServerError,
                    title: "Error al recuperar la contraseña");
            }
        }



    }
}