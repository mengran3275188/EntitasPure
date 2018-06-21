using DesperateDevs.Utils;
using System;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class MemberDataComponentDataProvider : IComponentDataProvider
	{
		public void Provide(Type type, ComponentData data)
		{
			MemberData[] memberInfos = (from info in type.GetPublicMemberInfos()
			select new MemberData(info.type.ToCompilableString(), info.name)).ToArray();
			data.SetMemberData(memberInfos);
		}
	}
}
