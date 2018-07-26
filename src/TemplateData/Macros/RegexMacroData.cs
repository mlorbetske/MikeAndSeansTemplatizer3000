using System.Collections.Generic;

namespace TemplateData.Macros
{
    public class RegexMacroData : BaseMacroData
    {
        public const string Identity = "regex";

        public override string Type => Identity;

        public string SourceVariable { get; set; }

        public IReadOnlyList<RegexMacroStepData> Steps { get; set; }
    }

    public class RegexMacroStepData
    {
        public string Regex { get; set; }
        public string Replacement { get; set; }
    }
}
