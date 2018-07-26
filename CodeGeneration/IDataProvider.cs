namespace Entitas.CodeGeneration
{
	public interface IDataProvider : ICodeGenerationPlugin
	{
		CodeGeneratorData[] GetData();
	}
}
