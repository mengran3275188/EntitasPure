using DesperateDevs.Utils;
using System;

namespace Entitas.CodeGeneration.Plugins
{
	public class ShouldGenerateComponentComponentDataProvider : IComponentDataProvider
	{
		public void Provide(Type type, ComponentData data)
		{
			bool flag = !type.ImplementsInterface<IComponent>();
			data.ShouldGenerateComponent(flag);
			if (flag)
			{
				data.SetObjectType(type.ToCompilableString());
			}
		}
	}
}
