using Simionic.Core;

namespace Simionic.CustomProfiles.ImportExport
{
    public class AircraftConfig
    {
        public static AircraftConfig FromProfile(Profile profile)
        {



            return null;
        }

        public int Id { get; init; }
        public string Name { get; set; }
        public IEnumerable<ConfigItem> ConfigItems { get; private set; }
        public object this[string configName] => ConfigItems.SingleOrDefault(x => x.Name.ToLower() == configName.ToLower())?.Value;

        public AircraftConfig(int id, string name, IEnumerable<ConfigItem> configItems)
        {
            Id = id;
            Name = name;
            ConfigItems = configItems;
        }
    }
}