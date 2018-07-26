using System;
using System.Collections.Generic;
using DesperateDevs.Utils;
using System.Linq;
using System.Text.RegularExpressions;

namespace Entitas.CodeGeneration
{
    public class Properties
    {
        private const string placeholderPattern = "\\${(.+?)}";

        private readonly Dictionary<string, string> _dict;

        public string[] keys
        {
            get
            {
                return this._dict.Keys.ToArray();
            }
        }

        public string[] values
        {
            get
            {
                return this._dict.Values.ToArray();
            }
        }

        public int count
        {
            get
            {
                return this._dict.Count;
            }
        }

        public string this[string key]
        {
            get
            {
                return Regex.Replace(this._dict[key], "\\${(.+?)}", delegate (Match match)
                {
                    string value = match.Groups[1].Value;
                    if (!this._dict.ContainsKey(value))
                    {
                        return "${" + value + "}";
                    }
                    return this._dict[value];
                });
            }
            set
            {
                this._dict[key.Trim()] = Properties.unescapedSpecialCharacters(value.Trim());
            }
        }

        public Properties()
            : this(string.Empty)
        {
        }

        public Properties(string properties)
        {
            properties = properties.ToUnixLineEndings();
            this._dict = new Dictionary<string, string>();
            string[] linesWithProperties = Properties.getLinesWithProperties(properties);
            this.addProperties(Properties.mergeMultilineValues(linesWithProperties));
        }

        public Properties(Dictionary<string, string> properties)
        {
            this._dict = new Dictionary<string, string>(properties);
        }

        public bool HasKey(string key)
        {
            return this._dict.ContainsKey(key);
        }

        public void AddProperties(Dictionary<string, string> properties, bool overwriteExisting)
        {
            foreach (KeyValuePair<string, string> property in properties)
            {
                if (overwriteExisting || !this.HasKey(property.Key))
                {
                    this[property.Key] = property.Value;
                }
            }
        }

        public void RemoveProperty(string key)
        {
            this._dict.Remove(key);
        }

        public Dictionary<string, string> ToDictionary()
        {
            return new Dictionary<string, string>(this._dict);
        }

        private void addProperties(string[] lines)
        {
            char[] keyValueDelimiter = new char[1]
            {
            '='
            };
            foreach (string[] item in from line in lines
                                      select line.Split(keyValueDelimiter, 2))
            {
                if (item.Length != 2)
                {
                    throw new InvalidKeyPropertiesException(item[0]);
                }
                this[item[0]] = item[1];
            }
        }

        private static string[] getLinesWithProperties(string properties)
        {
            char[] separator = new char[1]
            {
            '\n'
            };
            return (from line in properties.Split(separator, StringSplitOptions.RemoveEmptyEntries)
                    select line.TrimStart(' ') into line
                    where !line.StartsWith("#", StringComparison.Ordinal)
                    select line).ToArray();
        }

        private static string[] mergeMultilineValues(string[] lines)
        {
            string currentProperty = string.Empty;
            return lines.Aggregate(new List<string>(), delegate (List<string> acc, string line)
            {
                currentProperty += line;
                if (currentProperty.EndsWith("\\", StringComparison.Ordinal))
                {
                    currentProperty = currentProperty.Substring(0, currentProperty.Length - 1);
                }
                else
                {
                    acc.Add(currentProperty);
                    currentProperty = string.Empty;
                }
                return acc;
            }).ToArray();
        }

        private static string escapedSpecialCharacters(string str)
        {
            return str.Replace("\n", "\\n").Replace("\t", "\\t");
        }

        private static string unescapedSpecialCharacters(string str)
        {
            return str.Replace("\\n", "\n").Replace("\\t", "\t");
        }

        private static string propertyPair(string key, string value, bool minified)
        {
            if (minified)
            {
                return key + "=" + value;
            }
            return key + " = " + value;
        }

        public override string ToString()
        {
            return this._dict.Aggregate(string.Empty, delegate (string properties, KeyValuePair<string, string> kv)
            {
                string[] array = (from value in Properties.escapedSpecialCharacters(kv.Value).ArrayFromCSV()
                                  select value.PadLeft(kv.Key.Length + 3 + value.Length)).ToArray();
                string value2 = string.Join(", \\\n", array).TrimStart();
                return properties + Properties.propertyPair(kv.Key, value2, false) + ((array.Length > 1) ? "\n\n" : "\n");
            });
        }

        public string ToMinifiedString()
        {
            return this._dict.Aggregate(string.Empty, delegate (string properties, KeyValuePair<string, string> kv)
            {
                string value = Properties.escapedSpecialCharacters(kv.Value);
                return properties + Properties.propertyPair(kv.Key, value, true) + "\n";
            });
        }
    }
}
