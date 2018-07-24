namespace TemplateData.Symbols
{
    public class DerivedSymbolData : BaseValueSymbolData
    {
        public const string TypeName = "derived";

        public string ValueTransform { get; set; }

        public string ValueSource { get; set; }
    }
}
