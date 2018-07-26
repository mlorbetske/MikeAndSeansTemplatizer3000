using System.Collections.Generic;

namespace TemplateData.CustomOperations.OperationProviders
{
    public class ConditionalLineCommentOperationData : OperationBaseData
    {
        public const string OperationName = "conditional";
        public const string Style = "line";

        public string Token { get; set; }
        public IReadOnlyList<string> IfKeywords { get; set; }
        public IReadOnlyList<string> ElseIfKeywords { get; set; }
        public IReadOnlyList<string> ElseKeyworkds { get; set; }
        public IReadOnlyList<string> EndIfKeywords { get; set; }
        public string KeywordPrefix { get; set; }
        public string Evaluator { get; set; }
        public bool Trim { get; set; }
        public bool WholeLine { get; set; }
    }
}
