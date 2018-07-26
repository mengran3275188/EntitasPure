using System.Collections.Generic;
using System.Linq;

namespace Entitas.CodeGeneration.CodeGenerator
{
	public class CodeGenerator
	{
        private readonly IPreProcessor[] _preProcessors;

        private readonly IDataProvider[] _dataProviders;

        private readonly ICodeGenerator[] _codeGenerators;

        private readonly IPostProcessor[] _postProcessors;

        private readonly bool _trackHooks;

        private readonly Dictionary<string, object> _objectCache;

        private bool _cancel;

        public event GeneratorProgress OnProgress;

        public CodeGenerator(IPreProcessor[] preProcessors, IDataProvider[] dataProviders, ICodeGenerator[] codeGenerators, IPostProcessor[] postProcessors, bool trackHooks = true)
        {
            this._preProcessors = (from i in preProcessors
                                   orderby i.priority
                                   select i).ToArray();
            this._dataProviders = (from i in dataProviders
                                   orderby i.priority
                                   select i).ToArray();
            this._codeGenerators = (from i in codeGenerators
                                    orderby i.priority
                                    select i).ToArray();
            this._postProcessors = (from i in postProcessors
                                    orderby i.priority
                                    select i).ToArray();
            this._trackHooks = trackHooks;
            this._objectCache = new Dictionary<string, object>();
        }

        public CodeGenFile[] DryRun()
        {
            return this.generate("[Dry Run] ", (from i in this._preProcessors
                                                where i.runInDryMode
                                                select i).ToArray(), (from i in this._dataProviders
                                                                      where i.runInDryMode
                                                                      select i).ToArray(), (from i in this._codeGenerators
                                                                                            where i.runInDryMode
                                                                                            select i).ToArray(), (from i in this._postProcessors
                                                                                                                  where i.runInDryMode
                                                                                                                  select i).ToArray());
        }

        public CodeGenFile[] Generate()
        {
            CodeGenFile[] array = this.generate(string.Empty, this._preProcessors, this._dataProviders, this._codeGenerators, this._postProcessors);
            if (this._trackHooks)
            {
                this.trackHooks(array);
            }
            return array;
        }

        private void trackHooks(CodeGenFile[] files)
        {
            /*
            foreach (CodeGeneratorTrackingHook item in AppDomain.CurrentDomain.GetInstancesOf<ITrackingHook>().OfType<CodeGeneratorTrackingHook>())
            {
                item.Track(this._preProcessors, this._dataProviders, this._codeGenerators, this._postProcessors, files);
            }
            */
        }

        private CodeGenFile[] generate(string messagePrefix, IPreProcessor[] preProcessors, IDataProvider[] dataProviders, ICodeGenerator[] codeGenerators, IPostProcessor[] postProcessors)
        {
            this._cancel = false;
            this._objectCache.Clear();
            foreach (ICachable item in ((IEnumerable<ICodeGenerationPlugin>)preProcessors).Concat((IEnumerable<ICodeGenerationPlugin>)dataProviders).Concat(codeGenerators).Concat(postProcessors)
                .OfType<ICachable>())
            {
                item.objectCache = this._objectCache;
            }
            int num = preProcessors.Length + dataProviders.Length + codeGenerators.Length + postProcessors.Length;
            int num2 = 0;
            foreach (IPreProcessor preProcessor in preProcessors)
            {
                if (this._cancel)
                {
                    return new CodeGenFile[0];
                }
                num2++;
                if (this.OnProgress != null)
                {
                    this.OnProgress(messagePrefix + "Pre Processing", preProcessor.name, (float)num2 / (float)num);
                }
                preProcessor.PreProcess();
            }
            List<CodeGeneratorData> list = new List<CodeGeneratorData>();
            foreach (IDataProvider dataProvider in dataProviders)
            {
                if (this._cancel)
                {
                    return new CodeGenFile[0];
                }
                num2++;
                if (this.OnProgress != null)
                {
                    this.OnProgress(messagePrefix + "Creating model", dataProvider.name, (float)num2 / (float)num);
                }
                list.AddRange(dataProvider.GetData());
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
            foreach (IPostProcessor postProcessor in postProcessors)
            {
                if (this._cancel)
                {
                    return new CodeGenFile[0];
                }
                num2++;
                if (this.OnProgress != null)
                {
                    this.OnProgress(messagePrefix + "Post Processing", postProcessor.name, (float)num2 / (float)num);
                }
                array = postProcessor.PostProcess(array);
            }
            return array;
        }

        public void Cancel()
        {
            this._cancel = true;
        }
    }
}
