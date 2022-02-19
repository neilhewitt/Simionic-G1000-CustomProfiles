using System.Linq;
using System.Security.Claims;

namespace Simionic.CustomProfiles.Web
{
    public static class UserExtensions
    {
        public static string GetEmail(this ClaimsPrincipal user)
        {
            return user.Claims.SingleOrDefault(x => x.Type == "email")?.Value;
        }
    }
}
