namespace TemplateData.Macros
{
    public class RandomMacroData : BaseMacroData
    {
        public const string Identity = "random";

        public override string Type => Identity;

        public int Low { get; set; }

        public int High { get; set; }
    }
}
