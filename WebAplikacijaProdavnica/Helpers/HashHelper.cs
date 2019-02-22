using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace WebAplikacijaProdavnica.Helpers
{
    public class HashHelper
    {
        public static string Encrypt(string clearText)
        {
            try
            {
                byte[] hashBytes = ComputeHash(clearText);
                byte[] saltBytes = GetRandomSalt();
                byte[] saltHash = ComputeHash(saltBytes.ToString());

                byte[] hashWithSaltBytes = new byte[hashBytes.Length + saltBytes.Length];
                for (int i = 0; i < hashBytes.Length; i++)
                    hashWithSaltBytes[i] = hashBytes[i];
                for (int i = 0; i < saltBytes.Length; i++)
                    hashWithSaltBytes[hashBytes.Length + i] = saltBytes[i];

                string hashValue = Convert.ToBase64String(hashWithSaltBytes);

                return hashValue;
            }
            catch (Exception)
            {
                throw;
            }
        }

        //random salt generation
        public static byte[] GetRandomSalt()
        {
            int minSaltSize = 16;
            int maxSaltSize = 32;

            Random random = new Random();
            int saltSize = random.Next(minSaltSize, maxSaltSize);
            byte[] saltBytes = new byte[saltSize];
            RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
            rng.GetNonZeroBytes(saltBytes);
            return saltBytes;
        }
        // hashing
        public static byte[] ComputeHash(string plainText)
        {
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            HashAlgorithm hash = new SHA256Managed();
            return hash.ComputeHash(plainTextBytes);
        }
    }
}