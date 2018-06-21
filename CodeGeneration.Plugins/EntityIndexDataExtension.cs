using System.Collections.Generic;

namespace Entitas.CodeGeneration.Plugins
{
	public static class EntityIndexDataExtension
	{
		public const string ENTITY_INDEX_TYPE = "entityIndex_type";

		public const string ENTITY_INDEX_IS_CUSTOM = "entityIndex_isCustom";

		public const string ENTITY_INDEX_CUSTOM_METHODS = "entityIndex_customMethods";

		public const string ENTITY_INDEX_NAME = "entityIndex_name";

		public const string ENTITY_INDEX_CONTEXT_NAMES = "entityIndex_contextNames";

		public const string ENTITY_INDEX_KEY_TYPE = "entityIndex_keyType";

		public const string ENTITY_INDEX_COMPONENT_TYPE = "entityIndex_componentType";

		public const string ENTITY_INDEX_MEMBER_NAME = "entityIndex_memberName";

		public static string GetEntityIndexType(this EntityIndexData data)
		{
			return (string)((Dictionary<string, object>)data)["entityIndex_type"];
		}

		public static void SetEntityIndexType(this EntityIndexData data, string type)
		{
			((Dictionary<string, object>)data)["entityIndex_type"] = type;
		}

		public static bool IsCustom(this EntityIndexData data)
		{
			return (bool)((Dictionary<string, object>)data)["entityIndex_isCustom"];
		}

		public static void IsCustom(this EntityIndexData data, bool isCustom)
		{
			((Dictionary<string, object>)data)["entityIndex_isCustom"] = isCustom;
		}

		public static MethodData[] GetCustomMethods(this EntityIndexData data)
		{
			return (MethodData[])((Dictionary<string, object>)data)["entityIndex_customMethods"];
		}

		public static void SetCustomMethods(this EntityIndexData data, MethodData[] methods)
		{
			((Dictionary<string, object>)data)["entityIndex_customMethods"] = methods;
		}

		public static string GetEntityIndexName(this EntityIndexData data)
		{
			return (string)((Dictionary<string, object>)data)["entityIndex_name"];
		}

		public static void SetEntityIndexName(this EntityIndexData data, string name)
		{
			((Dictionary<string, object>)data)["entityIndex_name"] = name;
		}

		public static string[] GetContextNames(this EntityIndexData data)
		{
			return (string[])((Dictionary<string, object>)data)["entityIndex_contextNames"];
		}

		public static void SetContextNames(this EntityIndexData data, string[] contextNames)
		{
			((Dictionary<string, object>)data)["entityIndex_contextNames"] = contextNames;
		}

		public static string GetKeyType(this EntityIndexData data)
		{
			return (string)((Dictionary<string, object>)data)["entityIndex_keyType"];
		}

		public static void SetKeyType(this EntityIndexData data, string type)
		{
			((Dictionary<string, object>)data)["entityIndex_keyType"] = type;
		}

		public static string GetComponentType(this EntityIndexData data)
		{
			return (string)((Dictionary<string, object>)data)["entityIndex_componentType"];
		}

		public static void SetComponentType(this EntityIndexData data, string type)
		{
			((Dictionary<string, object>)data)["entityIndex_componentType"] = type;
		}

		public static string GetMemberName(this EntityIndexData data)
		{
			return (string)((Dictionary<string, object>)data)["entityIndex_memberName"];
		}

		public static void SetMemberName(this EntityIndexData data, string memberName)
		{
			((Dictionary<string, object>)data)["entityIndex_memberName"] = memberName;
		}
	}
}
