using DesperateDevs.Utils;
using System;

namespace Entitas.CodeGeneration.Plugins
{
	public class ComponentTypeComponentDataProvider : IComponentDataProvider
	{
		public void Provide(Type type, ComponentData data)
		{
			data.SetFullTypeName(type.ToCompilableString());
		}
	}
}
