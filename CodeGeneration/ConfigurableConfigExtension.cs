using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Entitas.CodeGeneration
{
    public static class ConfigurableConfigExtension
    {
        public static T CreateAndConfigure<T>(this Preferences preferences) where T : IConfigurable, new()
        {
            T result = new T();
            result.Configure(preferences);
            return result;
        }
    }
}
