using System;
using System.Security.Cryptography;

namespace LoginForm.Services
{
    public static class EncryptPassword
    {
        // These constants may be changed without breaking existing hashes.
        public const int SALT_BYTES = 24;
        public const int HASH_BYTES = 18;
        public const int PBKDF2_ITERATIONS = 64000;

        // These constants define the encoding and may not be changed.
        public const int HASH_SECTIONS = 5;
        public const int HASH_ALGORITHM_INDEX = 0;
        public const int ITERATION_INDEX = 1;
        public const int HASH_SIZE_INDEX = 2;
        public const int SALT_INDEX = 3;
        public const int PBKDF2_INDEX = 4;

        public static string CreateSALT()
        {
            // Generate a random salt
            byte[] salt = new byte[SALT_BYTES];
            using (RNGCryptoServiceProvider csprng = new RNGCryptoServiceProvider())
            {
                csprng.GetBytes(salt);
            }
            return Convert.ToBase64String(salt);
        }

        public static byte[] CreateHASH(string password, string salt)
        {
            var saltByte = Convert.FromBase64String(salt);
            //Generate hash from given password and string
            byte[] hash = PBKDF2(password, saltByte, PBKDF2_ITERATIONS, HASH_BYTES);
            return hash;
        }

        //Extension method tutorial
        public static byte[] ConvertStringToByte(this string salt)
        {
            return Convert.FromBase64String(salt);
        }

        private static byte[] PBKDF2(string password, byte[] salt, int iterations, int outputBytes)
        {
            //Main encryption function
            using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, salt))
            {
                pbkdf2.IterationCount = iterations;
                return pbkdf2.GetBytes(outputBytes);
            }
        }
    }
}
