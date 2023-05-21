using System;
using System.IO;
using System.Security.Cryptography;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;

namespace SteelGames.Models
{
    public static class GeneralUtils
    {
        public static string PathToImages = "C:\\Users\\sausm\\source\\repos\\SteelGames\\source\\images\\games\\";
        public static string ReportRepository = "C:\\khpi\\3 course\\appz2\\ReportRepo";

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

        public static bool VerifyPassword(string password, string hashedPassword)
        {
            // Extract the salt and password hash from the combined hash
            string[] hashParts = hashedPassword.Split(':');
            byte[] salt = Convert.FromBase64String(hashParts[0]);
            string storedHashedPassword = hashParts[1];

            // Hash the provided password with the extracted salt
            string hashedPasswordToVerify = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            // Compare the stored hashed password with the newly computed hash
            return storedHashedPassword.Equals(hashedPasswordToVerify);
        }

        public static void DeleteOldPreview(string pathToFile)
        {
            if(File.Exists(pathToFile))
            {
                File.Delete(pathToFile);
            }
        }
    }
}