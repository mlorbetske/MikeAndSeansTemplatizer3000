using System.Collections.Generic;
namespace TemplateData.Sources
{
    public class SourceModifierData
    {
        public string Condition { get; set; }
        public IReadOnlyList<string> CopyOnly { get; set; }
        public IReadOnlyList<string> Include { get; set; }
        public IReadOnlyList<string> Exclude { get; set; }
        public IReadOnlyDictionary<string, string> Rename { get; set; }
    }
}
