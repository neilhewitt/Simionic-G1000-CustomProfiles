using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Simionic.CustomProfiles.Web
{
    public static class UserInfo
    {
        public static string Name { get; set; }
        public static string Email { get; set; }
        public static string OwnerId { get; set; }
    }
}
