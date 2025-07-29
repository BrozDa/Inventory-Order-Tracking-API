using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Inventory_Order_Tracking.API.Utils
{
    public class PasswordHasher
    {
        private static string GenerateHash(string password, string salt)
        {
            var hash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: Convert.FromBase64String(salt),
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hash;
        }
        public static (string hash, string salt) GenerateHashAndSalt(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Password null or empty");
            }
            var salt = new byte[128];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(salt);

            var strSalt = Convert.ToBase64String(salt);

            var hash = GenerateHash(password, strSalt);

            return (hash, strSalt);
        }

        public static bool VerifyPassword(string hash, string password, string salt)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("Password null or empty");
            }

            var passwordHash = GenerateHash(password, salt);
            return (hash == passwordHash);
        }
    }
}
