using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace ZakaBank_24.Global_Classes
{
    public class clsUtil
    {
        /// <summary>
        /// Computes the SHA-256 hash value of the input string and returns it as a lowercase hexadecimal string.
        /// </summary>
        /// <param name="input">The input string to be hashed.</param>
        /// <returns>The SHA-256 hash value of the input string as a lowercase hexadecimal string.</returns>
        public static string ComputeHash(string input)
        {
            //SHA is Secutred Hash Algorithm.
            // Create an instance of the SHA-256 algorithm
            using (SHA256 sha256 = SHA256.Create())
            {
                // Compute the hash value from the UTF-8 encoded input string
                byte[] hashBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(input));


                // Convert the byte array to a lowercase hexadecimal string
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }

        /// <summary>
        /// Generates a random string of specified length using alphanumeric characters.
        /// </summary>
        /// <param name="length">The length of the random string to generate.</param>
        /// <returns>A random string of the specified length.</returns>
        public static string GetRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

    }
}