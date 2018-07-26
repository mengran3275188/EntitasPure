using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entitas.CodeGeneration
{
    public interface IConfigurable
    {
        Dictionary<string, string> defaultProperties
        {
            get;
        }
        void Configure(Preferences perferences);
    }
}
