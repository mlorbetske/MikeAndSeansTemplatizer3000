using System.Collections.Generic;

namespace TemplateData.ValueForms
{
    public class ChainValueFormData : ValueFormData
    {
        public const string FormName = "chain";

        public IReadOnlyList<string> Steps { get; set; }
    }
}
