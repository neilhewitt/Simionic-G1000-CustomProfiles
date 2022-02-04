namespace Simionic.CustomProfiles.Core
{
    public class Gauge
    {
        public string Name { get; init; }
        public double? Min { get; set; }
        public double? Max { get; set; }

        public IEnumerable<GaugeRange> Ranges { get; init; } = new GaugeRange[4];

        public Gauge(string name, double? min = null, double? max = null)
        {
            Name = name;
            Min = min;
            Max = max;
        }
    }

    public class FuelGauge : Gauge
    {
        public bool FuelInGallons { get; set; }
        public double CapacityForSingleTank { get; set; }
        public FuelGauge(bool fuelInGallons, double capacityForSingleTank, string name, double? min = null, double? max = null)
            : base(name, min, max)
        {
            FuelInGallons = fuelInGallons;
            CapacityForSingleTank = capacityForSingleTank;
        }
    }

    public class TorqueGauge : Gauge
    {
        public bool TorqueInFootPounds { get; set; }
        public TorqueGauge(bool torqueInFootPounds, string name, double? min = null, double? max = null)
            : base(name, min, max)
        {
            TorqueInFootPounds = torqueInFootPounds;
        }
    }
}
