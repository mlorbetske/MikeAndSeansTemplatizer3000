using System.Collections.Generic;

namespace TemplateData.Macros
{
    public class SwitchMacroData : BaseMacroData
    {
        public const string Identity = "switch";

        public override string Type => Identity;

        public IReadOnlyList<SwitchMacroSwitchesData> Switches { get; set; }
    }

    public class SwitchMacroSwitchesData
    {
        public string Condition { get; set; }
        public string Value { get; set; }
    }
}
