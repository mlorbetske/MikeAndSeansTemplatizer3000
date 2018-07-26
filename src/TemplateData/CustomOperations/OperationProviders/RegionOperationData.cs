namespace TemplateData.CustomOperations.OperationProviders
{
    public class RegionOperationData : OperationBaseData
    {
        public const string OperationName = "region";

        public string Start { get; set; }
        public string End { get; set; }
        public bool Include { get; set; }
        public bool Trim { get; set; }
        public bool WholeLine { get; set; }
    }
}
