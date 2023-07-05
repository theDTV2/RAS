using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;

namespace control.Helper
{
    public class HashHelper
    {
        static readonly byte[] salt = GenerateSalt();
        static readonly int numberOfBytes = salt.Length * 2;
        static readonly int numberOfIterations = 200000;

        public static byte[] GenerateSalt(int lenght = 64)
        {
            return RandomNumberGenerator.GetBytes(lenght);
        }

        public static string GenerateRandomBase64String(int lenght = 64)
        {
            return Convert.ToBase64String(RandomNumberGenerator.GetBytes(lenght));
        }

        public static string GetHashStringInBase64(string input)
        {
            return Convert.ToBase64String(CalculateHash(input));
        }

        public static bool CompareStringToHashString(string inputA, string hashStringB)
        {
            string hashedInputA = GetHashStringInBase64(inputA);

            if (hashedInputA.Equals(hashStringB))
                return true;
            return false;
        }
        public static bool CompareIntToHashString(int inputA, string hashStringB)
        {
            string inputStringA = GetHashStringInBase64(Convert.ToString(inputA));

            if (inputStringA.Equals(hashStringB))
                return true;
            return false;
        }

        private static byte[] CalculateHash(string input)
        {
            byte[] hash_output = KeyDerivation.Pbkdf2(
                input,
                salt,
                KeyDerivationPrf.HMACSHA512,
                numberOfIterations,
                numberOfBytes);

            return hash_output;
        }
    }
}
