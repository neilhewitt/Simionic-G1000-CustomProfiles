using System.Collections.Generic;
using System.Linq;

namespace IniFileManager
{
    public class Section
    {
        private IList<Entry> _entries;
        public string Name { get; set; }
        public IEnumerable<Entry> Entries => _entries;
        public IEnumerable<Entry> EntriesExcludingComments => _entries.Where(x => !x.IsComment);

        public Value this[string key]
        {
            get
            {
                return _entries.SingleOrDefault(x => x.Key.ToUpper() == key.ToUpper())?.Value;
            }
            set
            {
                Entry line = _entries.SingleOrDefault(x => x.Key.ToUpper() == key.ToUpper());
                if (line == null)
                {
                    line = new Entry(key, value, null);
                }
                else
                {
                    line.Value = new Value(value, null);
                }
                if (!_entries.Contains(line))
                {
                    _entries.Add(line);
                }
            }
        }

        public void AddComment(string commentLine)
        {
            _entries.Add(new Entry(commentLine));
        }

        public void AddEntry(string key, string value, char? valueDelimiter = null)
        {
            _entries.Add(new Entry(key, value, valueDelimiter));
        }

        public void RemoveEntry(Entry entry)
        {
            _entries.Remove(entry);
        }

        public Section(string name, IEnumerable<Entry> entries = null)
        {
            Name = name;
            _entries = new List<Entry>(entries ?? new Entry[0]);
        }
    }
}
