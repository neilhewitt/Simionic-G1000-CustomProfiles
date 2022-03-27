using System;
using System.Linq;

namespace IniFileManager
{
    public class Entry
    {
        public bool IsComment { get; private set; }
        public string Key { get; set; }
        public Value Value { get; set; }
        public string TrailingComment { get; set; }

        public override string ToString()
        {
            if (IsComment)
            {
                return Value.StringValue;
            }
            else if (Value != null && !String.IsNullOrWhiteSpace(Value.StringValue))
            {
                string value = Value.RawStringValue;
                return $"{Key}={value}{ (String.IsNullOrWhiteSpace(TrailingComment) ? "" : $" {TrailingComment}") }";
            }
            else
            {
                return Key;
            }
        }

        public Entry(string entry)
        {
            IsComment = true;
            Value = new Value(entry, null);
            Key = "";
        }

        public Entry(string key, string value, char? delimiter)
        {
            // if the value is followed by a comment, strip it off and stick it in TrailingComment
            if (value.Contains("//"))
            {
                string[] valueParts = value.Split(new string[] { "//" }, 1, StringSplitOptions.RemoveEmptyEntries);
                value = valueParts[0];
                TrailingComment = String.Join("//", valueParts.Skip(1));
            }

            Key = key;
            Value = new Value(value ?? "", delimiter);
        }
    }
}
