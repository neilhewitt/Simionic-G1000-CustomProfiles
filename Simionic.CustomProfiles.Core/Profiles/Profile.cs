using Newtonsoft.Json;

namespace Simionic.CustomProfiles.Core
{
    public class ProfileId
    {
        [JsonProperty("id")]
        public string Id { get; set; }
    }

    public class ProfileSummary : ProfileId
    {
        public OwnerInfo Owner { get; set; } = new OwnerInfo();
        public DateTime LastUpdated { get; set; }
        public string Name { get; set; }
        public AircraftType AircraftType { get; set; }
        public int Engines { get; set; } = 1;
        public bool IsPublished { get; set; }
        public string Notes { get; set; }
    }

    public class Profile : ProfileSummary
    {
        private bool _fadec;
        private bool _constantSpeed;

        [JsonIgnore]
        public Gauge[] Gauges => new Gauge[] { CHT, EGT, Torque, NG, ITT, ManifoldPressure, RPM, Fuel, TIT, FuelFlow, OilPressure, OilTemperature };

        public string ForkedFrom { get; set; }

        // Piston only
        public int Cylinders { get; set; } = 4;
        public bool FADEC { get { return _fadec; } set { _fadec = value; if (value) _constantSpeed = false; } }
        public bool Turbocharged { get; set; } = false;
        public bool ConstantSpeed { get { return _constantSpeed; } set { _constantSpeed = value; } }
        public VacuumPSIRange VacuumPSIRange { get; set; } = new VacuumPSIRange();
        public Gauge ManifoldPressure { get; set; } = new Gauge("Manifold Pressure", 0, 0);
        public Gauge CHT { get; set; } = new Gauge("CHT", 0, 0);
        public Gauge EGT { get; set; } = new Gauge("EGT", 0, 0);
        public Gauge TIT { get; set; } = new Gauge("TIT", 0, 0);
        public Gauge Load { get; set; } = new Gauge("Load %");

        // Turbo only
        public Gauge Torque { get; set; } = new Gauge("Torque", 0, 0, torqueInFootPounds: true);
        public Gauge NG { get; set; } = new Gauge("NG", null, null);

        // Turbo + Jet
        public Gauge ITT { get; set; } = new Gauge("ITT", 0, 0);

        public bool TemperaturesInFarenheit { get; set; } = false;
        public Gauge RPM { get; set; } = new Gauge("RPM", null, 0); // not jet
        public Gauge Fuel { get; set; } = new Gauge("Fuel", fuelInGallons: true, capacityForSingleTank: 0);
        public Gauge FuelFlow { get; set; } = new Gauge("Fuel Flow", null, 0);
        public Gauge OilPressure { get; set; } = new Gauge("Oil Pressure", null, 0);
        public Gauge OilTemperature { get; set; } = new Gauge("Oil Temperature", 0, 0);
        public bool DisplayElevatorTrim { get; set; }
        public SettingRange ElevatorTrimTakeOffRange { get; set; } = new SettingRange();
        public bool DisplayRudderTrim { get; set; }
        public SettingRange RudderTrimTakeOffRange { get; set; } = new SettingRange();
        public bool DisplayFlapsIndicator { get; set; }
        public FlapsRange FlapsRange { get; set; } = new FlapsRange();
        public VSpeeds VSpeeds { get; set; } = new VSpeeds();

        public override string ToString()
        {
            return $"{Name} ({AircraftType}) LastUpdated: {LastUpdated}";
        }

        public Profile()
        {
        }
    }
}
