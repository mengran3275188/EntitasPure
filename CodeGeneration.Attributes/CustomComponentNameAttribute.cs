using System;

namespace Entitas.CodeGeneration.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
	public class CustomComponentNameAttribute : Attribute
	{
		public readonly string[] componentNames;

		public CustomComponentNameAttribute(params string[] componentNames)
		{
			this.componentNames = componentNames;
		}
	}
}
