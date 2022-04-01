using Simionic.Core;
using System;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace Simionic.CustomProfiles.Web
{
    public static class User
    {
        public static string Name { get; set; }
        public static string Email { get; set; }
        public static string OwnerId { get; set; }

        public static bool IsLoggedIn => Email != null;

        public static bool Owns(Profile profile)
        {
            return (profile?.Owner?.Id != null && profile?.Owner?.Id == User.OwnerId);
        }

        public static bool Owns(ProfileSummary profileSummary)
        {
            return (profileSummary?.Owner?.Id != null && profileSummary?.Owner?.Id == User.OwnerId);
        }
    }
}
