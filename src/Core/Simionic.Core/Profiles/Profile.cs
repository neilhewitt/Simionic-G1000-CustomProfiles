using Newtonsoft.Json;

namespace Simionic.Core
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
        private int _cylinders;

        [JsonIgnore]
        public Gauge[] Gauges => new Gauge[] { CHT, EGT, Torque, NG, ITT, ManifoldPressure, Load, RPM, Fuel, TIT, FuelFlow, OilPressure, OilTemperature };

        public string ForkedFrom { get; set; }

        // Piston only
        public int Cylinders { get { return _cylinders; } set { _cylinders = (value is 0 or 4 or 6) ? value : 0; } } // ensure only valid values allowed - older versions of Simionic defaulted the value to 1
        public bool FADEC { get { return _fadec; } set { _fadec = value; if (value) _constantSpeed = false; } }
        public bool Turbocharged { get; set; } = false;
        public bool ConstantSpeed { get { return _constantSpeed; } set { _constantSpeed = value; } }
        public VacuumPSIRange VacuumPSIRange { get; set; } = new VacuumPSIRange();
        public Gauge ManifoldPressure { get; set; } = new Gauge("Manifold Pressure (inHg)", 0, 0, allowDecimals: true);
        public Gauge CHT { get; set; } = new Gauge("CHT (°F)", 0, 0);
        public Gauge EGT { get; set; } = new Gauge("EGT (°F)", 0, 0);
        public Gauge TIT { get; set; } = new Gauge("TIT (°F)", 0, 0);
        public Gauge Load { get; set; } = new Gauge("Load %");

        // Turbo only
        public Gauge Torque { get; set; } = new Gauge("Torque (FT-LB)", 0, 0, torqueInFootPounds: true);
        public Gauge NG { get; set; } = new Gauge("NG (RPM%)", null, null);

        // Turbo + Jet
        public Gauge ITT { get; set; } = new Gauge("ITT (°F)", 0, 0);

        // common to all
        public bool TemperaturesInFahrenheit { get; set; } = true;
        public Gauge RPM { get; set; } = new Gauge("RPM", null, 0); // not jet
        public Gauge Fuel { get; set; } = new Gauge("Fuel", fuelInGallons: true, capacityForSingleTank: 0);
        public Gauge FuelFlow { get; set; } = new Gauge("Fuel Flow (GPH)", null, 0, allowDecimals: true);
        public Gauge OilPressure { get; set; } = new Gauge("Oil Pressure (PSI)", null, 0, allowDecimals: true);
        public Gauge OilTemperature { get; set; } = new Gauge("Oil Temp (°F)", 0, 0);
        public bool DisplayElevatorTrim { get; set; }
        public SettingRange ElevatorTrimTakeOffRange { get; set; } = new SettingRange();
        public bool DisplayRudderTrim { get; set; }
        public SettingRange RudderTrimTakeOffRange { get; set; } = new SettingRange();
        public bool DisplayFlapsIndicator { get; set; }
        public FlapsRange FlapsRange { get; set; } = new FlapsRange();
        public VSpeeds VSpeeds { get; set; } = new VSpeeds();

        public void FixUpGauges()
        {
            ManifoldPressure.AllowDecimals = true;
            FuelFlow.AllowDecimals = true;
            OilPressure.AllowDecimals = true;

            FixRanges(ManifoldPressure);
            FixRanges(FuelFlow);
            FixRanges(OilPressure);

            void FixRanges(Gauge gauge)
            {
                for (int i = 0; i < 4; i++)
                {
                    gauge.Ranges[i].AllowDecimals = true;
                }
            }
        }

        public override string ToString()
        {
            return $"{Name}";
        }

        public Profile()
        {
            // special defaults
            AircraftType = AircraftType.Piston;
            Engines = 1;
            Cylinders = 4;
        }
    }
}
