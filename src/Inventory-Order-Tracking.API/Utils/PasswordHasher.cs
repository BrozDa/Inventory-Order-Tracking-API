using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;

namespace Inventory_Order_Tracking.API.Utils
{
    /// <summary>
    /// Provides password and salt generation/ verification capabilities for the application
    /// </summary>
    public class PasswordHasher
    {
        /// <summary>
        /// Generates hash and password salt
        /// </summary>
        /// <param name="password">Password to be hashed and for which salt to be generated</param>
        /// <returns>A tupple containing string representations of hash and salt</returns>
        /// <exception cref="ArgumentNullException">Throws when password is null or empty</exception>
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

        /// <summary>
        /// Checks whether password with combination with salt generates same hash 
        /// </summary>
        /// <param name="password">A password to be checked</param>
        /// <param name="hash">Stored password hash</param>
        /// <param name="salt">Stored password salt</param>
        /// <returns>A true if hash generated using passed password is same as the stored one, false otherwise</returns>
        public static bool VerifyPassword(string? password, string hash,  string salt)
        {
            if (string.IsNullOrEmpty(password))
            {
                return false;
            }

            var passwordHash = GenerateHash(password, salt);

            return (hash == passwordHash);
        }

        /// <summary>
        /// Generates hash based on password and hash 
        /// </summary>
        /// <param name="password">A password for which hash should be generated</param>
        /// <param name="salt">A salt to be used when generating hash</param>
        /// <returns>A string representation of generated hash</returns>
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
    }
}