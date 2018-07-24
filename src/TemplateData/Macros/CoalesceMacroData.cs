namespace TemplateData.Macros
{
    public class CoalesceMacroData : BaseMacroData
    {
        public const string Identity = "coalesce";

        public override string Type => Identity;

        public string SourceVariableName { get; set; }

        public string DefaultValue { get; set; }

        public string FallbackVariableName { get; set; }
    }
}
