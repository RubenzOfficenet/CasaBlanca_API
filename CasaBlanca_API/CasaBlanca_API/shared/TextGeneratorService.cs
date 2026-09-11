using System.Security.Cryptography;

namespace CasaBlanca_API.shared
{
    public static class TextGeneratorService
    {
        private const string Letters = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";

        /// <summary>
        /// Genera un texto aleatorio de exactamente 8 caracteres (solo letras).
        /// </summary>
        public static string GenerateRandomTextChars()
        {
            const int length = 12;
            char[] result = new char[length];

            for (int i = 0; i < length; i++)
            {
                int index = RandomNumberGenerator.GetInt32(0, Letters.Length);
                result[i] = Letters[index];
            }

            return new string(result);
        }
    }
}
