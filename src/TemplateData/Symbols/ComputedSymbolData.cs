using System;
using System.Collections.Generic;

namespace TemplateData.Symbols
{
    public class ComputedSymbolData : SymbolData
    {
        public const string TypeName = "computed";

        public string DataType { get; set; }

        public string Value { get; set; }

        public string Evaluator { get; set; }
    }
}
