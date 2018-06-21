using Entitas.CodeGeneration.Attributes;
using System;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class IsUniqueComponentDataProvider : IComponentDataProvider
	{
		public void Provide(Type type, ComponentData data)
		{
			bool isUnique = Attribute.GetCustomAttributes(type).OfType<UniqueAttribute>().Any();
			data.IsUnique(isUnique);
		}
	}
}
