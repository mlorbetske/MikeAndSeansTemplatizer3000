using System.Collections.Generic;

namespace TemplateData.CustomOperations
{
    public class VariableConfigData
    {
        public IReadOnlyDictionary<string, string> Sources { get; set; }

        public IReadOnlyList<string> Order { get; set; }

        public string FallbackFormat { get; set; }

        public bool Expand { get; set; }
    }
}
