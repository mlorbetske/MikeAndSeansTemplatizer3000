using System.Collections.Generic;

namespace TemplateData.Symbols
{
    public class ParameterSymbolData : BaseValueSymbolData
    {
        public const string TypeName = "parameter";

        public string DefaultIfOptionWithoutValue { get; set; }

        public IReadOnlyDictionary<string, string> ChoicesAndDescriptions { get; set; }
    }
}
