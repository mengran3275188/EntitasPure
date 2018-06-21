using System.Collections.Generic;

namespace Entitas.CodeGeneration.Plugins
{
	public static class MemberInfosComponentDataExtension
	{
		public const string COMPONENT_MEMBER_INFOS = "component_memberInfos";

		public static MemberData[] GetMemberData(this ComponentData data)
		{
			return (MemberData[])((Dictionary<string, object>)data)["component_memberInfos"];
		}

		public static void SetMemberData(this ComponentData data, MemberData[] memberInfos)
		{
			((Dictionary<string, object>)data)["component_memberInfos"] = memberInfos;
		}
	}
}
