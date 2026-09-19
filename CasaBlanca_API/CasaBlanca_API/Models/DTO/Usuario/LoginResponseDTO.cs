namespace CasaBlanca_API.Models.DTO.Usuario
{
    public class LoginResponseDTO
    {
        public int Id { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public bool DebeCambiarPassword { get; set; }
    }
}
