namespace TemplateData.Macros
{
    public class UnsupportedTypeMacroData : BaseMacroData
    {
        public override string Type => SpecifiedType;

        public string SpecifiedType { get; set; }

        public string RawConfig { get; set; }
    }
}
