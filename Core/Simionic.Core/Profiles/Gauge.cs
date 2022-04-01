using Newtonsoft.Json;

namespace Simionic.Core
{
    public class Gauge
    {
        private double? _min;
        private double? _max;

        public string Name { get; set; }
        public double? Min { get { return _min; } set { _min = (value.HasValue) ? (AllowDecimals ? value : Math.Truncate(value.Value)) : null; } }
        public double? Max { get { return _max; } set { _max = (value.HasValue) ? (AllowDecimals ? value : Math.Truncate(value.Value)) : null; } }
        public bool? FuelInGallons { get; set; }
        public double? CapacityForSingleTank { get; set; }
        public bool? TorqueInFootPounds { get; set; }
        public double? MaxPower { get; set; }
        public GaugeRange[] Ranges { get; init; }

        public bool AllowDecimals { get; set; }

        public Gauge(string name, double? min = null, double? max = null, bool? fuelInGallons = null, double? capacityForSingleTank = null, bool? torqueInFootPounds = null, double? maxPower = null, bool allowDecimals = false)
        {
            Name = name;
            _min = min;
            _max = max;
            AllowDecimals = allowDecimals;
            FuelInGallons = fuelInGallons;
            CapacityForSingleTank = capacityForSingleTank;
            TorqueInFootPounds = torqueInFootPounds;
            MaxPower = maxPower;

            Ranges = new GaugeRange[4];
            for (int i = 0; i < 4; i++)
            {
                Ranges[i] = new GaugeRange(RangeColour.None, 0, 0);
                Ranges[i].AllowDecimals = AllowDecimals;
            }
        }
    }

    public enum GaugeType
    {
        Standard,
        Fuel,
        Torque
    }
}
