using TemplateData.CustomOperations.OperationProviders;

namespace TemplateData.CustomOperations
{
    public class CustomOperationData
    {
        public string Type { get; set; }

        public string Condition { get; set; }

        public OperationBaseData Operation { get; set; }
    }
}
