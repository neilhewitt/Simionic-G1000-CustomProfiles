namespace Simionic.CustomProfiles.Core
{
    public class GaugeRange
    {
        public RangeColour Colour { get; set; }
        public double Min { get; set; }
        public double Max { get; set; }

        public GaugeRange(RangeColour colour, double min, double max)
        {
            Colour = colour;
            Min = min;
            Max = max;
        }
    }
}
