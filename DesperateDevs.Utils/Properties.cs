using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace DesperateDevs.Utils
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
				return Regex.Replace(this._dict[key], "\\${(.+?)}", (Match match) => this._dict[match.Groups[1].Value]);
			}
			set
			{
				this._dict[key.Trim()] = value.TrimStart().Replace("\\n", "\n").Replace("\\t", "\t");
			}
		}

		public Properties()
			: this(string.Empty)
		{
		}

		public Properties(string properties)
		{
			properties = Properties.convertLineEndings(properties);
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

		private static string convertLineEndings(string str)
		{
			return str.Replace("\r\n", "\n").Replace("\r", "\n");
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
			return lines.Aggregate(new List<string>(), delegate(List<string> acc, string line)
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

		public override string ToString()
		{
			return this._dict.Aggregate(string.Empty, delegate(string properties, KeyValuePair<string, string> kv)
			{
				string text = kv.Value.Replace("\n", "\\n").Replace("\t", "\\t");
				return properties + kv.Key + " = " + text + "\n";
			});
		}
	}
}
