namespace TemplateData.Macros
{
    public class CaseChangeMacroData : BaseMacroData
    {
        public const string Identity = "casing";

        public override string Type => Identity;

        public string SourceVariable { get; set; }

        // true means toLower, false means toUpper
        public bool ToLower { get; set; }
    }
}
