namespace TemplateData.Macros
{
    public class NowMacroData : BaseMacroData
    {
        public const string Identity = "now";

        public override string Type => Identity;

        public string Format { get; set; }

        public bool Utc { get; set; }
    }
}
