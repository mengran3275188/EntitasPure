namespace Entitas.CodeGeneration
{
	public interface ICodeGenerator : ICodeGeneratorInterface
	{
		CodeGenFile[] Generate(CodeGeneratorData[] data);
	}
}
