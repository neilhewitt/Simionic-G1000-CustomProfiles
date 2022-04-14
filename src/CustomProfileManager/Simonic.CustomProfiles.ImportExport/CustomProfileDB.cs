using System;
using System.Text;
using Microsoft.Data.Sqlite;
using Newtonsoft.Json;
using Simionic.Core;

namespace Simionic.CustomProfiles.ImportExport
{
    public static class ImportExportExtensions
    {
        public static string ToZeroOrOne(this bool boolean)
        {
            return boolean ? "1" : "0";
        }

        public static string Escape(this string value)
        {
            return value.Replace("\'", "\'\'");
        }

        public static void SaveAsJson(this Profile profile, string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                string json = JsonConvert.SerializeObject(profile, Formatting.Indented);
                File.WriteAllText(Path.Combine(folderPath, profile.Name.Replace(' ', '-') + ".json"), json);
            }
        }
    }

    public class CustomProfileDB
    {
        private List<Profile> _profiles;
        private List<AircraftConfig> _aircraft;
        private Dictionary<Profile, AircraftConfig> _configsByProfile;
        private List<(int AircraftId, Profile Profile)> _removed;
        
        private int _maxId;
        private int _newId;
        private string _dbPath;

        public string DatabasePath => _dbPath;
        public bool Loaded { get; private set; }
        public IEnumerable<Profile> Profiles => _profiles;

        public void SaveAllAsJson(string folderPath)
        {
            if (Directory.Exists(folderPath))
            {
                foreach (Profile profile in Profiles)
                {
                    string json = JsonConvert.SerializeObject(profile, Formatting.Indented);
                    File.WriteAllText(Path.Combine(folderPath, profile.Name.Replace(' ', '-') + ".json"), json);
                }
            }
        }

        public byte[] SaveToDatabase(bool updateExistingProfiles = true, string logPath = null)
        {
            // make a backup first...
            string backupFolder = Path.GetDirectoryName(_dbPath);
            string backupFilename = $"{Path.GetFileNameWithoutExtension(_dbPath)}_backup.db";
            File.Copy(_dbPath, Path.Combine(backupFolder, backupFilename), true);

            using (SqliteConnection connection = new SqliteConnection($"Data Source={_dbPath}"))
            {
                List<string> log = new();

                connection.Open();
                foreach (Profile profile in _profiles)
                {
                    int aircraftId = _configsByProfile[profile].Id;
                    if (aircraftId > _maxId)
                    {
                        // new
                        ExecuteCommand($"INSERT INTO Aircraft (ACNum, ACName) VALUES ({aircraftId},'{profile.Name.Escape()}')", connection, log);
                        _maxId = aircraftId;

                        // build SQL
                        StringBuilder sql = new StringBuilder("INSERT INTO ConfigItems (ACNum, ConfigName, ConfigValue) VALUES \n");
                        AircraftConfig config = new AircraftConfigBuilder(profile, aircraftId).AircraftConfig;
                        foreach (ConfigItem configItem in config.ConfigItems)
                        {
                            sql.Append($"({aircraftId}, '{configItem.Name}', '{Convert.ToString(configItem.Value).Escape()}'),\n");
                        }

                        ExecuteCommand(sql.ToString().Substring(0, sql.Length - 2), connection, log);
                    }
                    else if (updateExistingProfiles)
                    {
                        AircraftConfig config = new AircraftConfigBuilder(profile, aircraftId).AircraftConfig;
                        foreach (ConfigItem configItem in config.ConfigItems)
                        {
                            ExecuteCommand($"UPDATE ConfigItems SET ConfigValue = '{Convert.ToString(configItem.Value).Escape()}' WHERE ACNum = {aircraftId} AND ConfigName = '{configItem.Name.Escape()}'", connection, log);
                        }
                    }
                }

                foreach (var removedProfile in _removed)
                {
                    ExecuteCommand($"DELETE FROM ConfigItems WHERE ACNum = {removedProfile.AircraftId}", connection, log);
                    ExecuteCommand($"DELETE FROM Aircraft WHERE ACNum = {removedProfile.AircraftId}", connection, log);
                }

                //File.WriteAllLines(logPath, log);
            }

            string tempFileName = _dbPath + ".temp";
            File.Copy(_dbPath, tempFileName, true);
            byte[] data = File.ReadAllBytes(tempFileName);
            File.Delete(tempFileName);
            return data;

            void Add(IList<string> values, string name, object value, bool withQuotes = false)
            {
                string valueAsString = Convert.ToString(value);
                values.Add($"('{name}', {(withQuotes ? value : $"'{value}'")})");
            }
        }

        private void ExecuteCommand(string commandText, SqliteConnection connection, IList<string> log)
        {
            using (SqliteCommand command = new SqliteCommand(commandText, connection))
            {
                log.Add(commandText);
                command.ExecuteNonQuery();
            }
        }

        public void AddProfile(string name)
        {
            AddProfile(name, null);
        }

        public void AddProfile(Profile profile)
        {
            AddProfile(null, profile);
        }

        public bool FileIsValid(string jsonPath)
        {
            if (File.Exists(jsonPath))
            {
                try
                {
                    string json = File.ReadAllText(jsonPath);
                    Profile profile = JsonConvert.DeserializeObject<Profile>(json);
                    return true;
                }
                catch
                {
                }
            }

            return false;
        }

        public Profile ImportProfileFromJson(string jsonPath)
        {
            if (File.Exists(jsonPath))
            {
                string json = File.ReadAllText(jsonPath);
                Profile profile = JsonConvert.DeserializeObject<Profile>(json);
                AddProfile(profile);
                return profile;
            }
            else
            {
                throw new FileNotFoundException("Invalid path.");
            }
        }

        public void RemoveProfile(Profile profile)
        {
            int id = _configsByProfile[profile].Id;
            _removed.Add((id, profile));
            _configsByProfile.Remove(profile);
            _profiles.Remove(profile);
            if (_maxId == id) _maxId = _configsByProfile.Max(x => x.Value.Id);
        }

        public void RemoveProfile(string name)
        {
            RemoveProfile(_profiles.SingleOrDefault(x => x.Name == name));
        }

        private void AddProfile(string name = null, Profile profile = null)
        {
            if (profile == null) profile = new Profile() { Name = name ?? "New Profile" };
            AircraftConfig config = new AircraftConfigBuilder(profile, _newId++).AircraftConfig;
            _aircraft.Add(config);
            _profiles.Add(profile);
            _configsByProfile.Add(profile, config);
        }

        private void Load()
        {
            if (!Loaded)
            {
                using (SqliteConnection connection = new SqliteConnection($"Data Source={_dbPath}"))
                {
                    connection.Open();

                    List<ConfigItem> configItems = new List<ConfigItem>();
                    using (SqliteCommand command = new SqliteCommand("SELECT * FROM ConfigItems", connection))
                    {

                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                configItems.Add(new ConfigItem() { AircraftId = reader.GetInt32(0), Name = reader.GetString(1), Value = reader.GetValue(2) });
                            }

                            reader.Close();
                        }
                    }
                    
                    using (SqliteCommand command = new SqliteCommand("SELECT * FROM Aircraft", connection))
                    {
                        using (SqliteDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                _aircraft.Add(new AircraftConfig(reader.GetInt32(0), reader.GetString(1), configItems.Where(x => x.AircraftId == reader.GetInt32(0)).ToList()));
                            }

                            reader.Close();
                        }
                    }
                }

                _maxId = (_aircraft.Count > 0) ? _aircraft.Max(x => x.Id) : 0;
                _newId = _maxId + 1;

                List<Profile> profiles = new();
                foreach (AircraftConfig aircraft in _aircraft)
                {
                    Profile profile = new ProfileBuilder(aircraft).Profile;
                    _configsByProfile.Add(profile, aircraft);
                    _profiles.Add(profile);
                }

                Loaded = true;
            }
        }

        public CustomProfileDB(string path)
        {
            _profiles = new List<Profile>();
            _configsByProfile = new Dictionary<Profile, AircraftConfig>();
            _aircraft = new List<AircraftConfig>();
            _removed = new List<(int AircraftId, Profile Profile)>();

            if (File.Exists(path))
            {
                _dbPath = path;
                Load();
            }
            else
            {
                throw new IOException("Path not found.");
            }
        }
    }
}