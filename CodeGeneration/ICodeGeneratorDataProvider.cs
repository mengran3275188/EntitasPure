namespace Entitas.CodeGeneration
{
	public interface ICodeGeneratorDataProvider : ICodeGeneratorInterface
	{
		CodeGeneratorData[] GetData();
	}
}
