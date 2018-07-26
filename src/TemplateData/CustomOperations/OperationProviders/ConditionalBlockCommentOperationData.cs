using System.Collections.Generic;

namespace TemplateData.CustomOperations.OperationProviders
{
    public class ConditionalBlockCommentOperationData : OperationBaseData
    {
        public const string OperationName = "conditional";
        public const string Style = "block";

        public IReadOnlyList<string> StartToken { get; set; }
        public IReadOnlyList<string> EndToken { get; set; }
        public IReadOnlyList<string> PseudoEndToken { get; set; }
        public string KeywordPrefix { get; set; }
        public string Evaluator { get; set; }
        public bool Trim { get; set; }
        public bool WholeLine { get; set; }
    }
}
