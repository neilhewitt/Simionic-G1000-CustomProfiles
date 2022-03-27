using System;
using System.Collections.Generic;

namespace IniFileManager
{
    public class Value
    {
        public static implicit operator string(Value value) => value?.StringValue ?? "";
        public static implicit operator Value(string valueString) => new Value(valueString, null);

        private string _value;
        private char? _delimiter;

        public bool IsNumeric => NumericValue != null;
        public bool IsBoolean => _value.ToUpper() == "TRUE" || _value.ToUpper() == "FALSE";
        public string StringValue { get { return Clean(_value); } set { _value = Clean(value); } }
        public string RawStringValue { get { return _value; } set { _value = value; } }
        public double? NumericValue { get { try { return (double)(Convert.ChangeType(_value, typeof(double)) ?? null); } catch { return null; } } set { _value = value?.ToString() ?? null; } }
        public bool BooleanValue { get { return IsBoolean ? (_value.ToUpper() == "TRUE") : false; } set { _value = value ? "TRUE" : "FALSE"; } }
        public IEnumerable<string> SplitByDelimiter => _delimiter.HasValue ? _value.Split(_delimiter.Value) : new string[] { _value };

        public override string ToString()
        {
            return _value;
        }

        private string Clean(string value)
        {
            return value.Replace("<!--", "").Replace("-->", "").Replace("\r", "");
        }

        public Value(string value, char? delimiter)
        {
            _value = value;
            _delimiter = delimiter;
        }
    }
}
