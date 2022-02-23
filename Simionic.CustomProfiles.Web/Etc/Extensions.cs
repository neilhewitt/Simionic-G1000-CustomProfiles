using System;
using System.Linq;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using Simionic.CustomProfiles.Core;

namespace Simionic.CustomProfiles.Web
{
    public static class Extensions
    {
        public static bool IsAt(this NavigationManager navigation, string route)
        {
            return navigation.ToBaseRelativePath(navigation.Uri).StartsWith(route);
        }

        public static bool Try<TException>(Action action, out TException exception)
            where TException : Exception
        {
            try
            {
                action();
                exception = null;
                return true;
            }
            catch (TException ex)
            {
                exception = ex;
                return false;
            }
        }

        public static string QueryValue(this NavigationManager navigation, string name)
        {
            var query = QueryHelpers.ParseQuery(navigation.ToAbsoluteUri(navigation.Uri).Query);
            StringValues values = String.Empty;
            if (query.TryGetValue(name, out values)) return values.SingleOrDefault();
            return null;
        }
    }
}
