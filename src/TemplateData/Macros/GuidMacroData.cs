namespace TemplateData.Macros
{
    public class GuidMacroData : BaseMacroData
    {
        public const string Identity = "guid";

        public override string Type => Identity;

        public string Format { get; set; }
    }
}
