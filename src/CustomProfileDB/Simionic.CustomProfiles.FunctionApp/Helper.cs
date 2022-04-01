using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Simionic.CustomProfiles.FunctionApp
{
    public static class Helper
    {
        // we're using a single salt for all hashes... which would be *spectacularly* bad, except all we're doing
        // is hashing the email address. There are no passwords or PII in CosmosDB other than name and a hash of the email
        private const string CRYPTO_SALT = "AWBH+yXC3ba1vxMj3MrnuXKHikL2RDSX"; 
        
        private const int CRYPTO_ITERATIONS = 100000;
        private const int CRYPTO_BYTES = 24;

        public static string ProfileDB => Environment.GetEnvironmentVariable("ProfileDB");
        public static string ProfileContainer => Environment.GetEnvironmentVariable("ProfileContainer");

        public static string GetOwnerId(string email)
        {
            byte[] saltBytes = Convert.FromBase64String(CRYPTO_SALT);
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(email, saltBytes, CRYPTO_ITERATIONS);
            return BitConverter.ToString(pbkdf2.GetBytes(CRYPTO_BYTES)).Replace("-","");
        }
    }
}
