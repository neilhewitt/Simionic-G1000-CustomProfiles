using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simionic.CustomProfiles.DesktopApp
{
    public static class RegistryManager
    {
        private const string KEY_NAME = @"SOFTWARE\Simionic Custom Profile Manager";

        public static string Version { get => Read<string>("Version"); set => Write<string>("Version", value); }
        public static DateTime LastCheckForUpdate { get => DateTime.Parse(Read<string>("LastCheckForUpdate") ?? DateTime.MinValue.ToString()); set => Write<string>("LastCheckForUpdate", value.ToString()); }
        public static string LastFolderPath { get => Read<string>("LastFolderPath"); set => Write<string>("LastFolderPath", value); }

        private static T Read<T>(string name)
        {
            var key = Registry.CurrentUser.CreateSubKey(KEY_NAME);
            T value = (T)key.GetValue(name);
            key.Close();
            return value;
        }

        private static void Write<T>(string name, T value)
        {
            var key = Registry.CurrentUser.CreateSubKey(KEY_NAME);
            key.SetValue(name, value);
            key.Close();
        }
    }
}
