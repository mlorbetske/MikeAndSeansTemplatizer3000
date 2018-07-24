using System.Collections.Generic;
using TemplateData.ValueForms;

namespace TemplateData.Macros
{
    public class ProcessValueFormMacroData : BaseMacroData
    {
        public const string Identity = "processValueForm";

        public override string Type => Identity;

        public string SourceVariable { get; set; }

        public string FormName { get; set; }
    }
}
