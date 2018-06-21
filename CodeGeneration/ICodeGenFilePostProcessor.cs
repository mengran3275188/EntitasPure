namespace Entitas.CodeGeneration
{
	public interface ICodeGenFilePostProcessor : ICodeGeneratorInterface
	{
		CodeGenFile[] PostProcess(CodeGenFile[] files);
	}
}
