using System.Collections.Generic;

namespace TemplateData.CustomOperations
{
    public class VariableConfigData
    {
        public IReadOnlyList<VariableConfigSource> Sources { get; set; }

        public string FallbackFormat { get; set; }

        public bool Expand { get; set; }
    }
}
