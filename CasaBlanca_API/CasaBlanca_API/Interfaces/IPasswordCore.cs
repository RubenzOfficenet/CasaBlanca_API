namespace CasaBlanca_API.Interfaces
{
    public interface IPasswordCore
    {
        string GenerateStrongPassword(int length = 16);
    }
}
