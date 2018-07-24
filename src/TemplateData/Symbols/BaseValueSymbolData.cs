using System.Collections.Generic;

namespace TemplateData.Symbols
{
    public abstract class BaseValueSymbolData : SymbolData
    {
        // ??? should this be on SymbolData
        public string Description { get; set; }

        public string DefaultValue { get; set; }

        public SymbolValueFormData Forms { get; set; }

        public bool IsRequired { get; set; }

        public string DataType { get; set; }

        public string FileRename { get; set; }

        public string Binding { get; set; }

        public string Replaces { get; set; }

        public IReadOnlyList<ReplacementContextData> ReplacementContexts { get; set; }
    }
}
