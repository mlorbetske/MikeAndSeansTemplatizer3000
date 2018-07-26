using System.Collections.Generic;

namespace TemplateData.CustomOperations.OperationProviders
{
    public class ConditionalCustomOperationData : OperationBaseData
    {
        public const string OperationName = "conditional";
        // String.Empty is also acceptable in the config
        public const string Style = "custom";

        public IReadOnlyList<string> IfTokens { get; set; }
        public IReadOnlyList<string> ElseTokens { get; set; }
        public IReadOnlyList<string> ElseIfTokens { get; set; }
        public IReadOnlyList<string> ActionableIfTokens { get; set; }
        public IReadOnlyList<string> ActionableElseTokens { get; set; }
        public IReadOnlyList<string> ActionableElseIfTokens { get; set; }
        public IReadOnlyList<string> Actions { get; set; }
        public IReadOnlyList<string> EndIfTokens { get; set; }
        public string Evaluator { get; set; }
        public bool Trim { get; set; }
        public bool WholeLine { get; set; }
    }
}
