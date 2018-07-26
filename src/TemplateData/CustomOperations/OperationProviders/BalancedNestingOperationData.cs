namespace TemplateData.CustomOperations.OperationProviders
{
    public class BalancedNestingOperationData : OperationBaseData
    {
        public const string OperationName = "balancednesting";

        public string StartToken { get; set; }
        public string RealEndToken { get; set; }
        public string PseudoEndToken { get; set; }
        public string ResetFlag { get; set; }
    }
}
