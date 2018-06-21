using System;

namespace Entitas.CodeGeneration.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
	public class CustomPrefixAttribute : Attribute
	{
		public readonly string prefix;

		public CustomPrefixAttribute(string prefix)
		{
			this.prefix = prefix;
		}
	}
}
