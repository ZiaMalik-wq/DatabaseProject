using System;
using System.Security.Cryptography;
using System.Text;

namespace FYP_Management.HelperClasses
{
    public static class PasswordHelper
    {
        private const int SaltSize = 16; // 128 bit
        private const int HashSize = 20; // 160 bit
        private const int Iterations = 10000;

        // Hashes a password with a new salt
        public static (string hash, string salt) HashPassword(string password)
        {
            // Create salt
            byte[] saltBytes = new byte[SaltSize];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            var salt = Convert.ToBase64String(saltBytes);

            // Create hash using SHA256
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256))
            {
                var hash = Convert.ToBase64String(pbkdf2.GetBytes(HashSize));
                return (hash, salt);
            }
        }

        // Verifies a password against a stored hash and salt
        public static bool VerifyPassword(string password, string storedHash, string storedSalt)
        {
            byte[] saltBytes = Convert.FromBase64String(storedSalt);

            // Create hash using SHA256
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256))
            {
                byte[] hashToCompare = pbkdf2.GetBytes(HashSize);
                return Convert.ToBase64String(hashToCompare) == storedHash;
            }
        }
    }
}