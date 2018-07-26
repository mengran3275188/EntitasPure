using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entitas.CodeGeneration
{
    public interface IPostProcessor : ICodeGenerationPlugin
    {
        CodeGenFile[] PostProcess(CodeGenFile[] files);
    }
}
