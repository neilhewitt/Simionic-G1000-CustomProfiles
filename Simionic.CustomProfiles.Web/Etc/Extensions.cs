using System;
using Microsoft.AspNetCore.Components;
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
    }
}
