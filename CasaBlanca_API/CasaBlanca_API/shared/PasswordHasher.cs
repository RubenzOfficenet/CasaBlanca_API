using System.Security.Cryptography;
using System.Text;

namespace CasaBlanca_API.shared
{
    public static class PasswordHasher
    {
        private const string Uppercase = "ABCDEFGHJKLMNPQRSTUVWXYZ";
        private const string Lowercase = "abcdefghijkmnpqrstuvwxyz";
        private const string Digits = "23456789";
        private const string NonAlphanumeric = "!@#$%^&*()_+-=[]{}|;:,.<>?";


        /// <summary>
        /// Genera una contraseña fuerte derivada de un texto semilla de entrada.
        /// </summary>
        /// <param name="seedText">Texto base ingresado por el usuario.</param>
        /// <param name="length">Longitud deseada para la contraseña (mínimo 8).</param>
        public static string GenerateFromSeed(string seedText, int length = 64)
        {
            if (string.IsNullOrWhiteSpace(seedText))
            {
                throw new ArgumentException("El texto base no puede estar vacío.", nameof(seedText));
            }

            if (length < 8)
            {
                throw new ArgumentException("La longitud mínima de la contraseña debe ser de 8 caracteres.", nameof(length));
            }

            // 1. Derivar bytes pseudoaleatorios deterministas a partir del texto de entrada usando HMACSHA256
            byte[] salt = Encoding.UTF8.GetBytes("SystemFixedSaltKey_2026"); // Clave fija de derivación
            using var hmac = new HMACSHA256(salt);
            byte[] hashBytes = hmac.ComputeHash(Encoding.UTF8.GetBytes(seedText));

            var allCharSets = Uppercase + Lowercase + Digits + NonAlphanumeric;
            var passwordBuilder = new StringBuilder(length);

            // 2. Garantizar al menos un carácter de cada grupo obligatorio usando los primeros bytes del hash
            passwordBuilder.Append(GetMappedChar(Uppercase, hashBytes[0]));
            passwordBuilder.Append(GetMappedChar(Lowercase, hashBytes[1]));
            passwordBuilder.Append(GetMappedChar(Digits, hashBytes[2]));
            passwordBuilder.Append(GetMappedChar(NonAlphanumeric, hashBytes[3]));

            // 3. Rellenar la longitud restante mapeando los bytes del hash a todo el conjunto de caracteres
            for (int i = 4; i < length; i++)
            {
                byte byteValue = hashBytes[i % hashBytes.Length];
                passwordBuilder.Append(GetMappedChar(allCharSets, byteValue));
            }

            // 4. Mezclar la cadena de forma determinista usando valores derivados del hash
            return DeterministicShuffle(passwordBuilder.ToString(), hashBytes);
        }

        private static char GetMappedChar(string characterSet, byte byteValue)
        {
            int index = byteValue % characterSet.Length;
            return characterSet[index];
        }

        private static string DeterministicShuffle(string input, byte[] hashBytes)
        {
            char[] array = input.ToCharArray();
            int n = array.Length;

            for (int i = 0; i < n; i++)
            {
                int swapIndex = hashBytes[i % hashBytes.Length] % n;
                (array[i], array[swapIndex]) = (array[swapIndex], array[i]);
            }

            return new string(array);
        }
    }
}
