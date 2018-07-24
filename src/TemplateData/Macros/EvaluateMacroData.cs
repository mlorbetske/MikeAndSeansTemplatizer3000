namespace TemplateData.Macros
{
    public class EvaluateMacroData : BaseMacroData
    {
        public const string Identity = "evaluate";

        public override string Type => Identity;

        public string Value { get; set; }

        public string Evaluator { get; set; }
    }
}
