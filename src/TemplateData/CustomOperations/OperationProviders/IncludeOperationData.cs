namespace TemplateData.CustomOperations.OperationProviders
{
    public class IncludeOperationData : OperationBaseData
    {
        public const string OperationName = "include";

        public string Start { get; set; }
        public string End { get; set; }
    }
}
