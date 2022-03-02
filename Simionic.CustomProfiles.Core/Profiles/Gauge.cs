using Newtonsoft.Json;

namespace Simionic.CustomProfiles.Core
{
    public class Gauge
    {
        public string Name { get; set; }
        public double? Min { get; set; }
        public double? Max { get; set; }
        public bool? FuelInGallons { get; set; }
        public double? CapacityForSingleTank { get; set; }
        public bool? TorqueInFootPounds { get; set; }
        public double? MaxPower { get; set; }
        public GaugeRange[] Ranges { get; init; }

        public Gauge(string name, double? min = null, double? max = null, bool? fuelInGallons = null, double? capacityForSingleTank = null, bool? torqueInFootPounds = null)
        {
            Name = name;
            Min = min;
            Max = max;
            FuelInGallons = fuelInGallons;
            CapacityForSingleTank = capacityForSingleTank;
            TorqueInFootPounds = torqueInFootPounds;

            Ranges = new GaugeRange[4];
            for (int i = 0; i < 4; i++)
            {
                Ranges[i] = new GaugeRange(RangeColour.None, 0, 0);
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
