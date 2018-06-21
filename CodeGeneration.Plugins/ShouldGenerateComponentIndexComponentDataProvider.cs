using Entitas.CodeGeneration.Attributes;
using System;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class ShouldGenerateComponentIndexComponentDataProvider : IComponentDataProvider
	{
		public void Provide(Type type, ComponentData data)
		{
			data.ShouldGenerateIndex(this.getGenerateIndex(type));
		}

		private bool getGenerateIndex(Type type)
		{
			DontGenerateAttribute dontGenerateAttribute = Attribute.GetCustomAttributes(type).OfType<DontGenerateAttribute>().SingleOrDefault();
			if (dontGenerateAttribute != null)
			{
				return dontGenerateAttribute.generateIndex;
			}
			return true;
		}
	}
}
