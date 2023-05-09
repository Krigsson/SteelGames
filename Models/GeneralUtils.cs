using System;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SteelGames.Models
{
    public static class GeneralUtils
    {
        public static string PathToImages = "C:\\Users\\sausm\\source\\repos\\SteelGames\\source\\images\\games\\";

        public static string PasswordHasher(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            // Derive a 256-bit subkey (use a different iteration count or HMAC algorithm here if desired)
            string hashedPassword = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Combine the salt and password hash
            string combinedHash = $"{Convert.ToBase64String(salt)}:{hashedPassword}";

            return combinedHash;
        }
    }
}