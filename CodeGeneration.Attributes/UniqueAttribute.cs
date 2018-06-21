using System;

namespace Entitas.CodeGeneration.Attributes
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Interface)]
	public class UniqueAttribute : Attribute
	{
	}
}
