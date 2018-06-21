using System;
using System.Linq;
using System.Text;

namespace DesperateDevs.Utils
{
	public static class StringExtension
	{
		public static string UppercaseFirst(this string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}
			return char.ToUpper(str[0]).ToString() + str.Substring(1);
		}

		public static string LowercaseFirst(this string str)
		{
			if (string.IsNullOrEmpty(str))
			{
				return str;
			}
			return char.ToLower(str[0]).ToString() + str.Substring(1);
		}

		public static string ToUnixLineEndings(this string str)
		{
			return str.Replace("\r\n", "\n").Replace("\r", "\n");
		}

		public static string ToCSV(this string[] values)
		{
			return string.Join(", ", (from value in values
			where !string.IsNullOrEmpty(value)
			select value.Trim()).ToArray());
		}

		public static string[] ArrayFromCSV(this string values)
		{
			return (from value in values.Split(new char[1]
			{
				','
			}, StringSplitOptions.RemoveEmptyEntries)
			select value.Trim()).ToArray();
		}

		public static string ToSpacedCamelCase(this string text)
		{
			StringBuilder stringBuilder = new StringBuilder(text.Length * 2);
			stringBuilder.Append(char.ToUpper(text[0]));
			for (int i = 1; i < text.Length; i++)
			{
				if (char.IsUpper(text[i]) && text[i - 1] != ' ')
				{
					stringBuilder.Append(' ');
				}
				stringBuilder.Append(text[i]);
			}
			return stringBuilder.ToString();
		}
	}
}
