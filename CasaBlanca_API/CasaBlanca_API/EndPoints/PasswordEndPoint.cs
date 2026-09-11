using CasaBlanca_API.Models.DTO.Rol;
using CasaBlanca_API.shared;

namespace CasaBlanca_API.EndPoints
{
    public static class PasswordEndPoint
    {
        public static void ConfigurePasswordEndPoint(this WebApplication app)
        {
            var connectionString = app.Configuration.GetConnectionString("DefultConnection");

            app.MapGet("/api/GeneratePassword", GeneratePassword).WithName("GeneratePassword").Produces<IEnumerable<RolDTO>>(200).Produces(500);
        }

        public async static Task<IResult> GeneratePassword(string password)
        {
            try
            {
                string newPassword = string.Empty;

                newPassword = PasswordHasher.GenerateFromSeed(password);

                return Results.Text(newPassword);
            }
            catch (Exception ex)
            {
                return Results.InternalServerError($"An error occurred: {ex.Message}");
            }
        }


    }
}
