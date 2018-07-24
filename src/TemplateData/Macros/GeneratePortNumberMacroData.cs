namespace TemplateData.Macros
{
    public class GeneratePortNumberMacroData : BaseMacroData
    {
        public const string Identity = "port";

        public override string Type => Identity;

        public int Low { get; set; }

        public int High { get; set; }
    }
}
