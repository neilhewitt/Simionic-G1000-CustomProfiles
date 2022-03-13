using Simionic.CustomProfiles.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simionic.CustomProfiles.ImportExport
{
    public class AircraftConfigBuilder
    {
        private Profile _profile;
        private int _aircraftId;
        private List<ConfigItem> _config;
        private Dictionary<Gauge, string> _gaugeToPrefixMap;

        public AircraftConfig AircraftConfig => new AircraftConfig(_aircraftId, _profile.Name, _config);

        private void BuildMap()
        {
            _gaugeToPrefixMap = new Dictionary<Gauge, string>()
            {
                {  _profile.ManifoldPressure, "ManIn" },
                {  _profile.Load, "Load" },
                {  _profile.RPM, "RPM" },
                {  _profile.ITT, "ITT" },
                {  _profile.Torque, "Torque" },
                {  _profile.Fuel, "FuelQty" },
                {  _profile.FuelFlow, "FFlow" },
                {  _profile.TIT, "TIT" },
                {  _profile.OilTemperature, "OilTemp" },
                {  _profile.OilPressure, "OilPress" },
                {  _profile.CHT, "CHT" },
                {  _profile.EGT, "EGT" },
                {  _profile.NG, "Ng" }, // note lower-case 'g'
            };
        }

        private void SetConfig()
        {
            // basic info
            _config.Add(Config("engineType", (int)_profile.AircraftType));
            _config.Add(Config("engineNum", _profile.Engines - 1));
            _config.Add(Config("cylinderCount", _profile.Cylinders));
            _config.Add(Config("FEDEC", _profile.FADEC ? 1 : 0)); // note spelling error
            _config.Add(Config("Turbocharged", _profile.Turbocharged ? 1 : 0));
            _config.Add(Config("ContantSpeed", _profile.ConstantSpeed ? 1 : 0)); // note spelling error
            _config.Add(Config("GaugeTempUnit", _profile.TemperaturesInFahrenheit ? 0 : 1));
            _config.Add(Config("GaugeStyle", "0"));

            // vacuum
            _config.Add(Config("GaugeVacMin", _profile.VacuumPSIRange.Min));
            _config.Add(Config("GaugeVacMax", _profile.VacuumPSIRange.Min));
            _config.Add(Config("GaugeVacGreenMin", _profile.VacuumPSIRange.GreenStart));
            _config.Add(Config("GaugeVacGreenMax", _profile.VacuumPSIRange.GreenEnd));

            // elevator trim
            _config.Add(Config("GaugeTrim", _profile.DisplayElevatorTrim ? 1 : 0));
            _config.Add(Config("GaugeTrimGreenMin", _profile.ElevatorTrimTakeOffRange.Min));
            _config.Add(Config("GaugeTrimGreenMax", _profile.ElevatorTrimTakeOffRange.Max));

            // rudder trim
            _config.Add(Config("GaugeRudderTrim", _profile.DisplayRudderTrim ? 1 : 0));
            _config.Add(Config("GaugeRudderTrimGreenMin", _profile.RudderTrimTakeOffRange.Min));
            _config.Add(Config("GaugeRudderTrimGreenMax", _profile.RudderTrimTakeOffRange.Max));

            // flaps
            _config.Add(Config("GaugeFlaps", _profile.DisplayFlapsIndicator ? 1 : 0));
            for (int i = 0; i < 6; i++)
            {
                _config.Add(Config($"GaugeFlapsNotchText{i}", _profile.FlapsRange.Markings[i]));
                if (i > 0 && i < 5) _config.Add(Config($"GaugeFlapsNotchPos{i}", _profile.FlapsRange.Positions[i]));
            }

            // v-speeds
            _config.Add(Config("Vs0", _profile.VSpeeds.Vs0));
            _config.Add(Config("Vs1", _profile.VSpeeds.Vs1));
            _config.Add(Config("Vfe", _profile.VSpeeds.Vfe));
            _config.Add(Config("Vno", _profile.VSpeeds.Vno));
            _config.Add(Config("Vne", _profile.VSpeeds.Vne));
            _config.Add(Config("Vglide", _profile.VSpeeds.Vglide));
            _config.Add(Config("Vr", _profile.VSpeeds.Vr));
            _config.Add(Config("Vx", _profile.VSpeeds.Vx));
            _config.Add(Config("Vy", _profile.VSpeeds.Vy));

            // other gauges
            foreach (Gauge gauge in _profile.Gauges)
            {
                SetGauge(gauge);
            }
        }

        private void SetGauge(Gauge gauge)
        {
            _config.Add(Config(GetKey(gauge, "Min"), gauge.Min));
            _config.Add(Config(GetKey(gauge, "Max"), gauge.Max));

            // special cases - if no key, ignored
            if (gauge == _profile.Load) _config.Add(Config(GetKey(gauge, "MaxKw"), gauge.MaxPower));
            if (gauge == _profile.Torque) _config.Add(Config(GetKey(gauge, "Style"), gauge.TorqueInFootPounds ?? true ? 0 : 1));
            if (gauge == _profile.Fuel) _config.Add(Config("GaugeFuelUnit", gauge.FuelInGallons ?? true ? 0 : 1));
            if (gauge == _profile.Fuel) _config.Add(Config(GetKey(gauge, ""), gauge.CapacityForSingleTank ?? 0));

            for (int i = 0; i < 4; i++)
            {
                _config.Add(Config(GetKey(gauge, $"SecColor{i}"), (int)gauge.Ranges[i].Colour));
                _config.Add(Config(GetKey(gauge, $"SecMin{i}"), (int)gauge.Ranges[i].Min));
                _config.Add(Config(GetKey(gauge, $"SecMax{i}"), (int)gauge.Ranges[i].Max));
            }
        }

        private string GetKey(Gauge gauge, string value)
        {
            return $"Gauge{_gaugeToPrefixMap[gauge]}{value}";
        }

        private ConfigItem Config(string name, object value)
        {
            return new ConfigItem() { AircraftId = _aircraftId, Name = name, Value = value };
        }

        public AircraftConfigBuilder(Profile profile, int aircraftId)
        {
            _profile = profile;
            _aircraftId = aircraftId;
            _config = new List<ConfigItem>();
            BuildMap();
            SetConfig();
        }
    }
}
