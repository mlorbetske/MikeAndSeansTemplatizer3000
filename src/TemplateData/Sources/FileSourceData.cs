using System.Collections.Generic;

namespace TemplateData.Sources
{
    public class FileSourceData
    {
        public string Source { get; set; }

        public string Target { get; set; }

        public SourceModifierData TopLevelFileOperations { get; set; }

        public IReadOnlyList<SourceModifierData> Modifiers { get; set; }
    }
}
