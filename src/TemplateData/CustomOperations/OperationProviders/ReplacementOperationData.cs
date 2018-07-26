using System.Collections.Generic;

namespace TemplateData.CustomOperations.OperationProviders
{
    public class ReplacementOperationData : OperationBaseData
    {
        public const string OperationName = "replacement";

        public string Original { get; set; }
        public string Replacement { get; set; }
        public IReadOnlyList<TokenLookaroundData> OnlyIf { get; set; }
    }
}
