using System.Collections.Generic;
namespace TemplateData.Sources
{
    public class SourceModifierData
    {
        public string Condition { get; set; }
        public IReadOnlyList<string> CopyOnlyGlobs { get; set; }
        public IReadOnlyList<string> IncludeGlobs { get; set; }
        public IReadOnlyList<string> ExcludeGlobs { get; set; }
        public IReadOnlyDictionary<string, string> RenamePatterns { get; set; }
    }
}
