using System;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Simionic.CustomProfiles.Core;

namespace Simonic.CustomProfiles.ImportExport
{
    public class CustomProfileDB : IDisposable
    {
        private SqliteConnection _connection;
        private List<AircraftConfig> _aircraft;

        public bool Loaded { get; private set; }
        public IEnumerable<Profile> Profiles {get; private set; }

        public void Save(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                foreach (Profile profile in Profiles)
                {
                    string json = JsonConvert.SerializeObject(profile);
                    File.WriteAllText(Path.Combine(folderPath, profile.Name.Replace(' ', '-') + ".json"), json);
                }
            }
        }

        private void Load()
        {
            if (!Loaded)
            {
                _connection.Open();

                List<ConfigItem> configItems = new List<ConfigItem>();
                SqliteCommand command = _connection.CreateCommand();
                
                command.CommandText = "SELECT * FROM ConfigItems";
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        configItems.Add(new ConfigItem() { AircraftId = reader.GetInt32(0), Name = reader.GetString(1), Value = reader.GetValue(2) });
                    }
                }

                command.CommandText = "SELECT * FROM Aircraft";
                using (SqliteDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        _aircraft.Add(new AircraftConfig(reader.GetInt32(0), reader.GetString(1), configItems.Where(x => x.AircraftId == reader.GetInt32(0)).ToList()));
                    }
                }

                _connection.Close();

                List<Profile> profiles = new List<Profile>();
                foreach (AircraftConfig aircraft in _aircraft)
                {
                    profiles.Add(new ProfileBuilder(aircraft).Profile);
                }

                Profiles = profiles;
                Loaded = true;
            }
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }

        public CustomProfileDB(string path)
        {
            if (File.Exists(path))
            {
                _connection = new SqliteConnection($"Data Source={path}");
                _aircraft = new List<AircraftConfig>();
                Load();
            }
            else
            {
                throw new IOException("Path not found.");
            }
        }
    }
}