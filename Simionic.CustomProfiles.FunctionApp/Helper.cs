using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using System;

namespace Simionic.CustomProfiles.FunctionApp
{
    public static class Helper
    {
        public static string ProfileDB => Environment.GetEnvironmentVariable("ProfileDB");
        public static string ProfileContainer => Environment.GetEnvironmentVariable("ProfileContainer");
    }
}
