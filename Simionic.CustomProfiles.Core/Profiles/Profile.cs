using Newtonsoft.Json;

namespace Simionic.CustomProfiles.Core
{
    public class ProfileSummary
    {
        [JsonProperty("id")] // need this for CosmosDB to work properly
        public string Id { get; set; }
        public Owner Owner { get; set; }
        public DateTime LastUpdated { get; set; }
        public string Name { get; set; }
        public AircraftType AircraftType { get; set; }
        public bool IsTwinEngine { get; set; }
    }

    public class Profile : ProfileSummary
    {
        public Gauge[] Gauges => new Gauge[] { CHT, EGT, Torque, NG, ITT, RPM, Fuel, FuelFlow, OilPressure, OilTemperature };

        public string ForkedFrom { get; set; }

        // Piston only
        public int Cylinders { get; set; } = 4;
        public bool FADEC { get; set; } = false;
        public bool Turbocharged { get; set; } = false;
        public bool ConstantSpeed { get; set; } = false;
        public Gauge CHT { get; set; } = new Gauge("CHT", 0, 0);
        public Gauge EGT { get; set; } = new Gauge("EGT", 0, 0);


        // Turbo only
        public Gauge Torque { get; set; } = new Gauge("Torque", torqueInFootPounds: true);
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
        public SettingRange ElevatorTrimTakeOffRange { get; set; }
        public bool DisplayRudderTrim { get; set; }
        public SettingRange RudderTrimTakeOffRange { get; set; }
        public bool DisplayFlapsIndicator { get; set; }
        public FlapsRange FlapsRange { get; set; }
        public VSpeeds VSpeeds { get; set; }

        public Profile()
        {
            FlapsRange = new FlapsRange();
            VSpeeds = new VSpeeds();
            ElevatorTrimTakeOffRange = new SettingRange();
            RudderTrimTakeOffRange = new SettingRange();
        }
    }
}
