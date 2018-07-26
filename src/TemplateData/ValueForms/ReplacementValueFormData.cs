namespace TemplateData.ValueForms
{
    public class ReplacementValueFormData : ValueFormData
    {
        public const string FormName = "replace";

        // The regex to match
        public string Pattern { get; set; }

        public string Replacement { get; set; }
    }
}
