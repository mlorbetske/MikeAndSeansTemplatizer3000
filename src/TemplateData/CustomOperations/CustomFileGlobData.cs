using System.Collections.Generic;

namespace TemplateData.CustomOperations
{
    public class CustomFileGlobData
    {
        public string Glob { get; set; }

        public IReadOnlyList<CustomOperationData> Operations { get; set; }

        public VariableConfigData VariableFormat { get; set; }

        public string FlagPrefix { get; set; }
    }
}
