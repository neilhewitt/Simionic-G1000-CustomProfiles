using Newtonsoft.Json;

namespace Simionic.Core
{
    public class GaugeRange
    {
        private double _min;
        private double _max;

        public RangeColour Colour { get; set; }
        public double Min { get { return _min; } set { _min = AllowDecimals ? _min : Math.Truncate(_min); } }
        public double Max { get { return _max; } set { _max = AllowDecimals ? _max : Math.Truncate(_max); } }
        
        public bool AllowDecimals { get; set; }

        public GaugeRange(RangeColour colour, double min, double max)
        {
            Colour = colour;
            _min = min;
            _max = max;
        }
    }
}
