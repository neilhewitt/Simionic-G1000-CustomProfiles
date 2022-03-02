using Simionic.CustomProfiles.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simonic.CustomProfiles.ImportExport
{
    public class ProfileBuilder
    {
        private Profile _profile;

        public Profile Profile => _profile;

        private Dictionary<Gauge, string> _gaugeToPrefixMap { get; set; }

        private void BuildMap(AircraftConfig config)
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
                {  _profile.NG, "NG" },
            };
        }

        private void SetGauge(Gauge gauge, AircraftConfig config)
        {
            gauge.Min = GetValue<double>(gauge, config, "Min");
            gauge.Max = GetValue<double>(gauge, config, "Max");

            // special cases - if no key, ignored
            gauge.MaxPower = GetValue<double>(gauge, config, "MaxKw");
            gauge.TorqueInFootPounds = GetValue<double>(gauge, config, "TorqueStyle") == 0;
            gauge.FuelInGallons = GetValue<double>("", config, "FuelUnit") == 0;
            gauge.CapacityForSingleTank = GetValue<double>("", config, "FuelQty");

            for (int i = 0; i < 4; i++)
            {
                gauge.Ranges[i].Colour = (RangeColour)GetValue<int>(gauge, config, $"SecColor{i}");
                gauge.Ranges[i].Min = GetValue<double>(gauge, config, $"SecMin{i}");
                gauge.Ranges[i].Max = GetValue<double>(gauge, config, $"SecMax{i}");
            }
        }

        private void SetProfile(AircraftConfig config)
        {
            // basic info
            _profile.AircraftType = (AircraftType)GetValue<int>("", config, "engineType");
            _profile.Engines = GetValue<int>("", config, "engineNum") + 1;
            _profile.Cylinders = GetValue<int>("", config, "cylinderCount");
            _profile.FADEC = GetValue<int>("", config, "FEDEC") == 1; // NOTE: spelling mistake is in DB
            _profile.Turbocharged = GetValue<int>("", config, "Turbocharged") == 1;
            _profile.ConstantSpeed = GetValue<int>("", config, "ContantSpeed") == 1; // another spelling mistake in DB
            _profile.TemperaturesInFahrenheit = GetValue<int>("", config, "GaugeTempUnit") == 1;

            // vacuum
            _profile.VacuumPSIRange.Min = GetValue<double>("GaugeVac", config, "Min");
            _profile.VacuumPSIRange.Min = GetValue<double>("GaugeVac", config, "Max");
            _profile.VacuumPSIRange.GreenStart = GetValue<double>("GaugeVac", config, "GreenMin");
            _profile.VacuumPSIRange.GreenEnd = GetValue<double>("GaugeVac", config, "GreenMax");

            // elevator trim
            _profile.DisplayElevatorTrim = GetValue<int>("Gauge", config, "Trim") == 1;
            _profile.ElevatorTrimTakeOffRange.Min = GetValue<double>("GaugeTrimGreen", config, "Min");
            _profile.ElevatorTrimTakeOffRange.Max = GetValue<double>("GaugeTrimGreen", config, "Max");

            // rudder trim
            _profile.DisplayRudderTrim = GetValue<int>("GaugeRudder", config, "Trim") == 1;
            _profile.RudderTrimTakeOffRange.Min = GetValue<double>("GaugeRudderTrimGreen", config, "Min");
            _profile.RudderTrimTakeOffRange.Max = GetValue<double>("GaugeRudderTrimGreen", config, "Max");

            // flaps
            _profile.DisplayFlapsIndicator = GetValue<int>("Gauge", config, "Flaps") == 1;
            for (int i = 0; i < 6; i++)
            {
                _profile.FlapsRange.Markings[i] = GetValue<string>("GaugeFlapsNotch", config, $"Text{i}");
                if (i > 0 && i < 5) _profile.FlapsRange.Positions[i] = GetValue<int>("GaugeFlapsNotch", config, $"Pos{i}");
            }

            // v-speeds
            _profile.VSpeeds.Vs0 = GetValue<int>("", config, "Vs0");
            _profile.VSpeeds.Vs1 = GetValue<int>("", config, "Vs1");
            _profile.VSpeeds.Vfe = GetValue<int>("", config, "Vfe");
            _profile.VSpeeds.Vno = GetValue<int>("", config, "Vno");
            _profile.VSpeeds.Vne = GetValue<int>("", config, "Vne");
            _profile.VSpeeds.Vglide = GetValue<int>("", config, "Vglide");
            _profile.VSpeeds.Vr = GetValue<int>("", config, "Vr");
            _profile.VSpeeds.Vx = GetValue<int>("", config, "Vx");
            _profile.VSpeeds.Vy = GetValue<int>("", config, "Vy");

            // other gauges
            foreach (Gauge gauge in _profile.Gauges)
            {
                SetGauge(gauge, config);
            }
        }

        private T GetValue<T>(Gauge gauge, AircraftConfig config, string name)
        {
            string prefix = $"Gauge{_gaugeToPrefixMap[gauge]}";
            return GetValue<T>(prefix, config, name);
        }

        private T GetValue<T>(string prefix, AircraftConfig config, string name)
        {
            name = $"{prefix}{name}";
            object value = config[name];
            if (value == null) return default(T);
            return (T)Convert.ChangeType(value, typeof(T));
        }

        public ProfileBuilder(AircraftConfig config)
        {
            _profile = new Profile();
            _profile.Name = config.Name;
            BuildMap(config);
            SetProfile(config);
        }
    }
}
