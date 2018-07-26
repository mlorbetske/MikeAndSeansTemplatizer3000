using System.Collections.Generic;
using TemplateData.Macros;

namespace TemplateData.Symbols
{
    public class GeneratedSymbolData : SymbolData
    {
        public const string TypeName = "generated";

        public string DataType { get; set; }

        public string Binding { get; set; }

        public string Replaces { get; set; }

        public string Generator { get; set; }

        //public IReadOnlyDictionary<string, BaseMacroData> Parameters { get; set; }
        public BaseMacroData MacroConfig { get; set; }

        public IReadOnlyList<ReplacementContextData> ReplacementContexts { get; set; }
    }
}
