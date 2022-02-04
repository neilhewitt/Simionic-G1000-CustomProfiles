using Newtonsoft.Json;

namespace Simionic.CustomProfiles.Core
{
    public class ProfileSummary
    {
        [JsonProperty("id")] // need this for CosmosDB to work properly
        public string Id { get; set; }
        public string Owner { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Name { get; set; }
        public AircraftType AircraftType { get; set; }
    }

    public class Profile : ProfileSummary
    {
        public string ForkedFrom { get; set; }

        public bool IsTwinEngine {get; set; }

        // Piston only
        public int Cylinders { get; set; } = 4;
        public bool FADEC { get; set; } = false;
        public bool Turbocharged { get; set; } = false;
        public bool ConstantSpeed { get; set; } = false;

        // Turbo only
        public TorqueGauge Torque { get; private set; } = new TorqueGauge(true, "TRQ", 0, 0);
        public Gauge NG { get; private set; } = new Gauge("NG", null, null);

        // Turbo + Jet
        public Gauge ITT { get; private set; } = new Gauge("ITT", 0, 0);

        public bool TemperaturesInFarenheit { get; set; } = false;
        public Gauge RPM { get; protected set; } = new Gauge("RPM", null, 0);
        public FuelGauge Fuel { get; protected set; } = new FuelGauge(true, 0, "Fuel Range", null, 0);
        public Gauge OilPressure { get; protected set; } = new Gauge("Oil Pressure", null, 0);
        public Gauge OilTemperature { get; protected set; } = new Gauge("Oil Temperature", 0, 0);
        public bool DisplayElevatorTrim { get; set; }
        public SettingRange ElevatorTrimTakeOffRange {get; protected set; }
        public bool DisplayRudderTrim { get; set; }
        public SettingRange RudderTrimTakeOffRange {get; protected set; }
        public bool DisplayFlapsIndicator { get; set; }
        public FlapsRange FlapsRange { get; protected set; }
        public VSpeeds VSpeeds { get; protected set; }

        public Profile()
        {
        }
    }
}
