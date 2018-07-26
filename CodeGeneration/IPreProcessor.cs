using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entitas.CodeGeneration
{
    public interface IPreProcessor : ICodeGenerationPlugin
    {
        void PreProcess();
    }
}
