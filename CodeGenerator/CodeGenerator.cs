using System.Collections.Generic;
using System.Linq;

namespace Entitas.CodeGeneration.CodeGenerator
{
	public class CodeGenerator
	{
		private readonly ICodeGeneratorDataProvider[] _dataProviders;

		private readonly ICodeGenerator[] _codeGenerators;

		private readonly ICodeGenFilePostProcessor[] _postProcessors;

		private bool _cancel;

		public event GeneratorProgress OnProgress;

		public CodeGenerator(ICodeGeneratorDataProvider[] dataProviders, ICodeGenerator[] codeGenerators, ICodeGenFilePostProcessor[] postProcessors)
		{
			this._dataProviders = (from i in dataProviders
			orderby i.priority
			select i).ToArray();
			this._codeGenerators = (from i in codeGenerators
			orderby i.priority
			select i).ToArray();
			this._postProcessors = (from i in postProcessors
			orderby i.priority
			select i).ToArray();
		}

		public CodeGenFile[] DryRun()
		{
			return this.generate("[Dry Run] ", (from i in this._dataProviders
			where i.runInDryMode
			select i).ToArray(), (from i in this._codeGenerators
			where i.runInDryMode
			select i).ToArray(), (from i in this._postProcessors
			where i.runInDryMode
			select i).ToArray());
		}

		public CodeGenFile[] Generate()
		{
			return this.generate(string.Empty, this._dataProviders, this._codeGenerators, this._postProcessors);
		}

		private CodeGenFile[] generate(string messagePrefix, ICodeGeneratorDataProvider[] dataProviders, ICodeGenerator[] codeGenerators, ICodeGenFilePostProcessor[] postProcessors)
		{
			this._cancel = false;
			List<CodeGeneratorData> list = new List<CodeGeneratorData>();
			int num = dataProviders.Length + codeGenerators.Length + postProcessors.Length;
			int num2 = 0;
			foreach (ICodeGeneratorDataProvider codeGeneratorDataProvider in dataProviders)
			{
				if (this._cancel)
				{
					return new CodeGenFile[0];
				}
				num2++;
				if (this.OnProgress != null)
				{
					this.OnProgress(messagePrefix + "Creating model", codeGeneratorDataProvider.name, (float)num2 / (float)num);
				}
				list.AddRange(codeGeneratorDataProvider.GetData());
			}
			List<CodeGenFile> list2 = new List<CodeGenFile>();
			CodeGeneratorData[] data = list.ToArray();
			foreach (ICodeGenerator codeGenerator in codeGenerators)
			{
				if (this._cancel)
				{
					return new CodeGenFile[0];
				}
				num2++;
				if (this.OnProgress != null)
				{
					this.OnProgress(messagePrefix + "Creating files", codeGenerator.name, (float)num2 / (float)num);
				}
				list2.AddRange(codeGenerator.Generate(data));
			}
			CodeGenFile[] array = list2.ToArray();
			foreach (ICodeGenFilePostProcessor codeGenFilePostProcessor in postProcessors)
			{
				if (this._cancel)
				{
					return new CodeGenFile[0];
				}
				num2++;
				if (this.OnProgress != null)
				{
					this.OnProgress(messagePrefix + "Processing files", codeGenFilePostProcessor.name, (float)num2 / (float)num);
				}
				array = codeGenFilePostProcessor.PostProcess(array);
			}
			return array;
		}

		public void Cancel()
		{
			this._cancel = true;
		}
	}
}
