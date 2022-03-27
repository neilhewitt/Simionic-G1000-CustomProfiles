using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace IniFileManager
{
    public class IniFile
    {
        private string _path;
        private List<Section> _sections = new List<Section>();
        public IEnumerable<Section> Sections => _sections;

        public Section this[string sectionName] => Sections.SingleOrDefault(x => x.Name.ToUpper() == sectionName.ToUpper());

        public Section AddSection(string sectionName)
        {
            Section section = new Section(sectionName);
            _sections.Add(section);
            return section;
        }

        public void RemoveSection(Section section)
        {
            _sections.Remove(section);
        }

        public void Read(string iniFilePath, char? valueDelimiter = null)
        {
            if (File.Exists(iniFilePath))
            {
                _path = iniFilePath;

                string[] lines = File.ReadAllText(iniFilePath).Split('\n');
                string sectionName = null;
                List<Entry> linesInSection = null;
                string multiLineKey = null;
                List<string> multiLineValue = new List<string>();
                bool inMultiLine = false;
                foreach(string line in lines)
                {
                    string currentLine = line.Replace("\t", "");

                    if (currentLine.StartsWith("["))
                    {
                        if (linesInSection != null)
                        {
                            Section section = new Section(sectionName, linesInSection);
                            _sections.Add(section);
                        }

                        sectionName = currentLine.Trim('[', ']', '\r').ToUpper();
                        linesInSection = new List<Entry>();
                    }
                    else if (!String.IsNullOrWhiteSpace(currentLine) && currentLine != "\n")
                    {
                        Entry entry = null;
                        if (currentLine.StartsWith("//") && !inMultiLine)
                        {
                            entry = new Entry(currentLine);  // comment line
                        }
                        else
                        {
                            if (inMultiLine)
                            {
                                if (currentLine.EndsWith($"-->\r"))
                                {
                                    string value = String.Join("", multiLineValue) + currentLine;
                                    entry = new Entry(multiLineKey, value, valueDelimiter);
                                    inMultiLine = false;
                                }
                                else
                                {
                                    multiLineValue.Add(currentLine);
                                }
                            }
                            else
                            {
                                string[] lineParts = currentLine.Split(new char[] { '=' }, 2);
                                if (lineParts.Length == 2)
                                {
                                    string key = lineParts[0].Trim();
                                    string value = lineParts[1].Trim();

                                    if (value.StartsWith("<!--") && !value.EndsWith($"-->"))
                                    {
                                        inMultiLine = true;
                                        multiLineKey = key;
                                        multiLineValue.Clear();
                                        multiLineValue.Add(value + '\r');
                                    }

                                    if (!inMultiLine)
                                    {
                                        entry = new Entry(key, value, valueDelimiter);
                                    }
                                }
                                else
                                {
                                    entry = new Entry(currentLine, null, valueDelimiter);
                                }
                            }
                        }
                        
                        if (entry != null) linesInSection?.Add(entry);
                    }
                }

                if (linesInSection != null)
                {
                    Section section = new Section(sectionName, linesInSection);
                    _sections.Add(section);
                }
            }
            else
            {
                throw new FileNotFoundException("Specified INI file does not exist.");
            }
        }

        public void Write(string iniFilePath = null)
        {
            List<string> entries = new List<string>();
            foreach(Section section in Sections)
            {
                entries.Add($"[{ section.Name }]");
                foreach(Entry entry in section.Entries)
                {
                    entries.Add(entry.ToString());
                }
                entries.Add(String.Empty);
            }

            File.WriteAllLines(iniFilePath ?? _path, entries);
        }

        public override string ToString()
        {
            List<string> lines = new List<string>();
            foreach (Section section in Sections)
            {
                lines.Add($"[{ section.Name }]");
                foreach (Entry line in section.Entries)
                {
                    lines.Add(line.ToString());
                }
                lines.Add(String.Empty);
            }

            string output = String.Join("\n", lines);
            return output;
        }

        public IniFile()
        {
        }
    }
}
