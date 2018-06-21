using Entitas.CodeGeneration.Attributes;
using System;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class UniquePrefixComponentDataProvider : IComponentDataProvider
	{
		public void Provide(Type type, ComponentData data)
		{
			data.SetUniqueComponentPrefix(this.getUniqueComponentPrefix(type));
		}

		private string getUniqueComponentPrefix(Type type)
		{
			CustomPrefixAttribute customPrefixAttribute = Attribute.GetCustomAttributes(type).OfType<CustomPrefixAttribute>().SingleOrDefault();
			if (customPrefixAttribute != null)
			{
				return customPrefixAttribute.prefix;
			}
			return "is";
		}
	}
}
