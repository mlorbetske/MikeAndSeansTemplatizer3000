namespace TemplateData.Macros
{
    public class RegexMatchMacroData : BaseMacroData
    {
        public const string Identity = "regexMatch";

        public override string Type => Identity;

        public string SourceVariable { get; set; }

        public string Pattern { get; set; }
    }
}
