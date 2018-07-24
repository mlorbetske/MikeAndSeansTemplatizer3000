using System.Collections.Generic;

namespace TemplateData.Macros
{
    public class RegexMacroData : BaseMacroData
    {
        public const string Identity = "regex";

        public override string Type => Identity;

        public string SourceVariable { get; set; }

        // Regex -> Replacement
        public IList<KeyValuePair<string, string>> Steps { get; set; }
    }
}
