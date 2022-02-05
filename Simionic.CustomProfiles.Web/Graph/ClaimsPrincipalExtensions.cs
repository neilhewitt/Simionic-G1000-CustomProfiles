using Microsoft.Graph;
using System;
using System.IO;
using System.Security.Claims;

namespace Simionic.CustomProfiles.Web.Graph
{
    public static class ClaimsPrincipalExtensions
    {
        public static string Email(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal.FindFirst("graph_email")?.Value;
        public static string PhotoURL(this ClaimsPrincipal claimsPrincipal) => claimsPrincipal.FindFirst("graph_photo")?.Value;

        public static void AddGraphClaims(this ClaimsPrincipal claimsPrincipal, User user)
        {
            var identity = claimsPrincipal.Identity as ClaimsIdentity;
            identity.AddClaim(new Claim("graph_email", user.Mail ?? user.UserPrincipalName));
        }

        public static void AddUserGraphPhoto(this ClaimsPrincipal claimsPrincipal, Stream photoStream)
        {
            var identity = claimsPrincipal.Identity as ClaimsIdentity;
            if (photoStream != null)
            {
                var memoryStream = new MemoryStream();
                photoStream.CopyTo(memoryStream);
                var photoBytes = memoryStream.ToArray();
                var photoUri = $"data:image/png;base64,{Convert.ToBase64String(photoBytes)}";
                identity.AddClaim(new Claim("graph_photo", photoUri));
            }
        }
    }
}
// </GraphClaimsExtensionsSnippet>
