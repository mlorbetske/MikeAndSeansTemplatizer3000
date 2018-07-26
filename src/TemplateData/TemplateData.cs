using System;
using System.Collections.Generic;
using TemplateData.BaseLines;
using TemplateData.CustomOperations;
using TemplateData.PostActions;
using TemplateData.PrimaryOutputs;
using TemplateData.Sources;
using TemplateData.Symbols;
using TemplateData.ValueForms;

namespace TemplateData
{
    public class TemplateData
    {
        public string Author { get; set; }
        public IReadOnlyList<string> Classifications { get; set; }
        public string DefaultName { get; set; }
        public string Description { get; set; }
        public string GroupIdentity { get; set; }
        public int Precedence { get; set; }
        public IReadOnlyList<Guid> Guids { get; set; }
        public string Identity { get; set; }
        public string Name { get; set; }
        public string SourceName { get; set; }
        public bool PreferNameDirectory { get; set; }
        public string PlaceholderFilename { get; set; }
        public string GeneratorVersions { get; set; }

        // note: also handle as string (short name)
        public IReadOnlyList<string> ShortNameList { get; set; }

        // ??? Should this be a list, since the name is on the ValueFormData
        public IReadOnlyDictionary<string, ValueFormData> Forms { get; set; }

        public IReadOnlyList<FileSourceData> Sources { get; set; }

        // List ??? ???
        public IReadOnlyDictionary<string, BaseLineData> BaselineInfo { get; set; }

        // deprecated (goes into _tagsDeprecated in SCM)
        public IReadOnlyDictionary<string, string> Tags { get; set; }

        public IReadOnlyDictionary<string, SymbolData> Symbols { get; set; }

        public IReadOnlyList<PostActionData> PostActions { get; set; }

        public IReadOnlyList<PrimaryOutputData> PrimaryOutputs { get; set; }

        public CustomFileGlobData CustomOperations { get; set; }

        public IReadOnlyList<CustomFileGlobData> GlobSpecificCustomOperations { get; set; }
    }
}
