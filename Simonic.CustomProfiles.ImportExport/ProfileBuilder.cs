using Simionic.CustomProfiles.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Simionic.CustomProfiles.ImportExport
{
    public class ProfileBuilder
    {
        private Profile _profile;

        public Profile Profile => _profile;

        private Dictionary<Gauge, string> _gaugeToPrefixMap;

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
                {  _profile.NG, "NG" },
            };
        }

        private void SetGauge(Gauge gauge, AircraftConfig config)
        {
            gauge.Min = GetValue<double>(gauge, config, "Min");
            gauge.Max = GetValue<double>(gauge, config, "Max");

            // special cases - if no key, ignored
            if (gauge == _profile.Load) gauge.MaxPower = GetValue<double>(gauge, config, "MaxKw");
            if (gauge == _profile.Torque) gauge.TorqueInFootPounds = GetValue<double>(gauge, config, "TorqueStyle") == 0;
            if (gauge == _profile.Fuel) gauge.FuelInGallons = GetValue<double>(config, "GaugeFuelUnit") == 0;
            if (gauge == _profile.Fuel) gauge.CapacityForSingleTank = GetValue<double>(config, "GaugeFuelQty");

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
            _profile.AircraftType = (AircraftType)GetValue<int>(config, "engineType");
            _profile.Engines = GetValue<int>(config, "engineNum") + 1;
            _profile.Cylinders = GetValue<int>(config, "cylinderCount");
            _profile.FADEC = GetValue<int>(config, "FEDEC") == 1; // NOTE: spelling mistake is in DB
            _profile.Turbocharged = GetValue<int>(config, "Turbocharged") == 1;
            _profile.ConstantSpeed = GetValue<int>(config, "ContantSpeed") == 1; // another spelling mistake in DB
            _profile.TemperaturesInFahrenheit = GetValue<int>(config, "GaugeTempUnit") == 1;

            // vacuum
            _profile.VacuumPSIRange.Min = GetValue<double>(config, "GaugeVacMin");
            _profile.VacuumPSIRange.Min = GetValue<double>(config, "GaugeVac");
            _profile.VacuumPSIRange.GreenStart = GetValue<double>(config, "GaugeVacGreenMin");
            _profile.VacuumPSIRange.GreenEnd = GetValue<double>(config, "GaugeVacGreenMax");

            // elevator trim
            _profile.DisplayElevatorTrim = GetValue<int>(config, "GaugeTrim") == 1;
            _profile.ElevatorTrimTakeOffRange.Min = GetValue<int>(config, "GaugeTrimGreenMin");
            _profile.ElevatorTrimTakeOffRange.Max = GetValue<int>(config, "GaugeTrimGreenMax");

            // rudder trim
            _profile.DisplayRudderTrim = GetValue<int>(config, "GaugeRudderTrim") == 1;
            _profile.RudderTrimTakeOffRange.Min = GetValue<int>(config, "GaugeRudderTrimGreenMin");
            _profile.RudderTrimTakeOffRange.Max = GetValue<int>(config, "GaugeRudderTrimGreenMax");

            // flaps
            _profile.DisplayFlapsIndicator = GetValue<int>(config, "GaugeFlaps") == 1;
            for (int i = 0; i < 6; i++)
            {
                _profile.FlapsRange.Markings[i] = GetValue<string>(config, $"GaugeFlapsNotchText{i}");
                if (i > 0 && i < 5) _profile.FlapsRange.Positions[i] = GetValue<int>(config, $"GaugeFlapsNotchPos{i}");
            }

            // v-speeds
            _profile.VSpeeds.Vs0 = GetValue<int>(config, "Vs0");
            _profile.VSpeeds.Vs1 = GetValue<int>(config, "Vs1");
            _profile.VSpeeds.Vfe = GetValue<int>(config, "Vfe");
            _profile.VSpeeds.Vno = GetValue<int>(config, "Vno");
            _profile.VSpeeds.Vne = GetValue<int>(config, "Vne");
            _profile.VSpeeds.Vglide = GetValue<int>(config, "Vglide");
            _profile.VSpeeds.Vr = GetValue<int>(config, "Vr");
            _profile.VSpeeds.Vx = GetValue<int>(config, "Vx");
            _profile.VSpeeds.Vy = GetValue<int>(config, "Vy");

            // other gauges
            foreach (Gauge gauge in _profile.Gauges)
            {
                SetGauge(gauge, config);
            }
        }

        private T GetValue<T>(Gauge gauge, AircraftConfig config, string name)
        {
            string prefix = $"Gauge{_gaugeToPrefixMap[gauge]}";
            return GetValue<T>(config, name, prefix);
        }

        private T GetValue<T>(AircraftConfig config, string name, string prefix = null)
        {
            name = $"{prefix ?? ""}{name}";
            object value = config[name];
            if (value is null) return default(T);
            if (value is string && String.IsNullOrWhiteSpace((string)value)) return default(T);

            return (T)Convert.ChangeType(value, typeof(T));
        }

        public ProfileBuilder(AircraftConfig config)
        {
            _profile = new Profile();
            _profile.Name = config.Name;
            BuildMap();
            SetProfile(config);
        }
    }
}
