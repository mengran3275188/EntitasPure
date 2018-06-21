using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;

namespace DesperateDevs.Utils
{
	public static class SerializationTypeExtension
	{
		private static readonly Dictionary<string, string> _builtInTypesToString = new Dictionary<string, string>
		{
			{
				"System.Boolean",
				"bool"
			},
			{
				"System.Byte",
				"byte"
			},
			{
				"System.SByte",
				"sbyte"
			},
			{
				"System.Char",
				"char"
			},
			{
				"System.Decimal",
				"decimal"
			},
			{
				"System.Double",
				"double"
			},
			{
				"System.Single",
				"float"
			},
			{
				"System.Int32",
				"int"
			},
			{
				"System.UInt32",
				"uint"
			},
			{
				"System.Int64",
				"long"
			},
			{
				"System.UInt64",
				"ulong"
			},
			{
				"System.Object",
				"object"
			},
			{
				"System.Int16",
				"short"
			},
			{
				"System.UInt16",
				"ushort"
			},
			{
				"System.String",
				"string"
			},
			{
				"System.Void",
				"void"
			}
		};

		private static readonly Dictionary<string, string> _builtInTypeStrings = new Dictionary<string, string>
		{
			{
				"bool",
				"System.Boolean"
			},
			{
				"byte",
				"System.Byte"
			},
			{
				"sbyte",
				"System.SByte"
			},
			{
				"char",
				"System.Char"
			},
			{
				"decimal",
				"System.Decimal"
			},
			{
				"double",
				"System.Double"
			},
			{
				"float",
				"System.Single"
			},
			{
				"int",
				"System.Int32"
			},
			{
				"uint",
				"System.UInt32"
			},
			{
				"long",
				"System.Int64"
			},
			{
				"ulong",
				"System.UInt64"
			},
			{
				"object",
				"System.Object"
			},
			{
				"short",
				"System.Int16"
			},
			{
				"ushort",
				"System.UInt16"
			},
			{
				"string",
				"System.String"
			},
			{
				"void",
				"System.Void"
			}
		};

		public static string ToCompilableString(this Type type)
		{
			if (SerializationTypeExtension._builtInTypesToString.ContainsKey(type.FullName))
			{
				return SerializationTypeExtension._builtInTypesToString[type.FullName];
			}
			if (type.IsGenericType)
			{
				string str = type.FullName.Split('`')[0];
				string[] value = (from argType in type.GetGenericArguments()
				select argType.ToCompilableString()).ToArray();
				return str + "<" + string.Join(", ", value) + ">";
			}
			if (type.IsArray)
			{
				return type.GetElementType().ToCompilableString() + "[" + new string(',', type.GetArrayRank() - 1) + "]";
			}
			if (type.IsNested)
			{
				return type.FullName.Replace('+', '.');
			}
			return type.FullName;
		}

		public static Type ToType(this string typeString)
		{
			string text = SerializationTypeExtension.generateTypeString(typeString);
			Type type = Type.GetType(text);
			if (type != null)
			{
				return type;
			}
			Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
			for (int i = 0; i < assemblies.Length; i++)
			{
				type = assemblies[i].GetType(text);
				if (type != null)
				{
					return type;
				}
			}
			return null;
		}

		public static string ShortTypeName(this string fullTypeName)
		{
			string[] array = fullTypeName.Split('.');
			return array[array.Length - 1];
		}

		public static string RemoveDots(this string fullTypeName)
		{
			return fullTypeName.Replace(".", string.Empty);
		}

		private static string generateTypeString(string typeString)
		{
			if (SerializationTypeExtension._builtInTypeStrings.ContainsKey(typeString))
			{
				typeString = SerializationTypeExtension._builtInTypeStrings[typeString];
			}
			else
			{
				typeString = SerializationTypeExtension.generateGenericArguments(typeString);
				typeString = SerializationTypeExtension.generateArray(typeString);
			}
			return typeString;
		}

		private static string generateGenericArguments(string typeString)
		{
			string[] separator = new string[1]
			{
				", "
			};
			typeString = Regex.Replace(typeString, "<(?<arg>.*)>", delegate(Match m)
			{
				string text = SerializationTypeExtension.generateTypeString(m.Groups["arg"].Value);
				int num = text.Split(separator, StringSplitOptions.None).Length;
				return "`" + num + "[" + text + "]";
			});
			return typeString;
		}

		private static string generateArray(string typeString)
		{
			typeString = Regex.Replace(typeString, "(?<type>[^\\[]*)(?<rank>\\[,*\\])", delegate(Match m)
			{
				string str = SerializationTypeExtension.generateTypeString(m.Groups["type"].Value);
				string value = m.Groups["rank"].Value;
				return str + value;
			});
			return typeString;
		}
	}
}
