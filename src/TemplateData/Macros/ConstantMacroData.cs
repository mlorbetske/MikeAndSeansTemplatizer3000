namespace TemplateData.Macros
{
    public class ConstantMacroData : BaseMacroData
    {
        public const string Identity = "constant";

        public override string Type => Identity;

        public string Value { get; set; }
    }
}
