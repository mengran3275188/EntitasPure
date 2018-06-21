using Entitas.CodeGeneration.Attributes;
using System;
using System.Linq;

namespace Entitas.CodeGeneration.Plugins
{
	public class ShouldGenerateMethodsComponentDataProvider : IComponentDataProvider
	{
		public void Provide(Type type, ComponentData data)
		{
			bool generate = !Attribute.GetCustomAttributes(type).OfType<DontGenerateAttribute>().Any();
			data.ShouldGenerateMethods(generate);
		}
	}
}
