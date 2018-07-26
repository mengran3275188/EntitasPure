using System.Collections.Generic;
using DesperateDevs.Utils;
using System.Linq;

namespace Entitas.CodeGeneration
{
	public class CodeGeneratorData : Dictionary<string, object>
    {
        public CodeGeneratorData()
        {
        }
        public CodeGeneratorData(CodeGeneratorData data)
            :base((IDictionary<string, object>)data)
        {
        }
        public string ReplacePlaceholders(string template)
        {
            return this.Aggregate(template, (string current, KeyValuePair<string, object> kv) => this.ReplacePlaceholders(current, kv.Key, kv.Value.ToString()));
        }

        public string ReplacePlaceholders(string template, string key, string value)
        {
            string oldValue = string.Format("${{{0}}}", key.UppercaseFirst());
            string oldValue2 = string.Format("${{{0}}}", key.LowercaseFirst());
            string newValue = value.UppercaseFirst();
            string newValue2 = value.LowercaseFirst();
            template = template.Replace(oldValue, newValue);
            template = template.Replace(oldValue2, newValue2);
            return template;
        }
    }
}
