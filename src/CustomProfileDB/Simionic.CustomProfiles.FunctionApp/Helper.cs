using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Simionic.Core;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace Simionic.CustomProfiles.FunctionApp
{
    public static class Helper
    {
        // we're using a single salt for all hashes... which would be *spectacularly* bad, except that
        // there are no passwords or PII in CosmosDB other than the name of the profile author,
        // and a hash of their email address. The email hash is used to find the profiles associated with that user.

        // The email is only available at runtime *after* a user has logged in, which is entirely handled by Microsoft, not by us. Therefore there is
        // no way for an attacker to get the email addresses of site users in order to brute-force the hashes. Unless, of course, Microsft Identity
        // gets hacked, and then we're all in much deeper trouble...

        private const string CRYPTO_SALT = "AWBH+yXC3ba1vxMj3MrnuXKHikL2RDSX"; 
        
        private const int CRYPTO_ITERATIONS = 100000;
        private const int CRYPTO_BYTES = 24;

        public static string ProfileDB => Environment.GetEnvironmentVariable("ProfileDB");
        public static string ProfileContainer => Environment.GetEnvironmentVariable("ProfileContainer");

        public static string GetOwnerId(string email)
        {
            // TODO: update to a more secure hashing algorithm (not sure yet how to do this in a way that doesn't break existing hashes)
            byte[] saltBytes = Convert.FromBase64String(CRYPTO_SALT);
            Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(email, saltBytes, CRYPTO_ITERATIONS);
            return BitConverter.ToString(pbkdf2.GetBytes(CRYPTO_BYTES)).Replace("-","");
        }
    }
}
