using System.Collections.Generic;

namespace TemplateData.BaseLines
{
    public class BaseLineData
    {
        public string Description { get; set; }
        public IReadOnlyDictionary<string, string> DefaultOverrides { get; set; }
    }
}
