using System.Security.Cryptography;

namespace CasaBlanca_API.shared
{
    public static class PasswordHasher
    {
        private const int SaltSize = 16;     // 128 bits
        private const int KeySize = 32;      // 256 bits
        private const int Iterations = 100000;
        private static readonly HashAlgorithmName Algorithm = HashAlgorithmName.SHA256;

        /// <summary>
        /// Genera un hash de la contraseña.
        /// </summary>
        public static string HashPassword(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);

            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                Algorithm,
                KeySize);

            return string.Join('.',
                Convert.ToBase64String(salt),
                Convert.ToBase64String(hash));
        }

        /// <summary>
        /// Verifica si la contraseña coincide con el hash.
        /// </summary>
        public static bool VerifyPassword(string password, string passwordHash)
        {
            var parts = passwordHash.Split('.');

            if (parts.Length != 2)
                return false;

            byte[] salt = Convert.FromBase64String(parts[0]);
            byte[] hash = Convert.FromBase64String(parts[1]);

            byte[] inputHash = Rfc2898DeriveBytes.Pbkdf2(
                password,
                salt,
                Iterations,
                Algorithm,
                KeySize);

            return CryptographicOperations.FixedTimeEquals(hash, inputHash);
        }

        internal static string PasswordHash(string? password)
        {
            throw new NotImplementedException();
        }
    }
}
