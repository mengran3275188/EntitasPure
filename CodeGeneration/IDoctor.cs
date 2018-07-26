using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entitas.CodeGeneration
{
    public interface IDoctor : ICodeGenerationPlugin
    {
        Diagnosis Diagnose();
        bool Fix();
    }
}
