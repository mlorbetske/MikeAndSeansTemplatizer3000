using System.Collections.Generic;

namespace TemplateData.Macros
{
    public class SwitchMacroData : BaseMacroData
    {
        public const string Identity = "switch";

        public override string Type => Identity;

        // condition -> value
        public IList<KeyValuePair<string, string>> Switches { get; set; }
    }
}
