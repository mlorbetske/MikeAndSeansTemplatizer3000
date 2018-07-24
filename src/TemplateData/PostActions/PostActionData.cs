using System;
using System.Collections.Generic;

namespace TemplateData.PostActions
{
    public class PostActionData
    {
        public string Condition { get; set; }

        public string Description { get; set; }

        public Guid ActionId { get; set; }

        public bool ContinueOnError { get; set; }

        // Future: if/when we explicitly define schema for the various post actions, deal with them here (in children?)
        // Note: the arg values can be objects (serialize them)
        public IReadOnlyDictionary<string, string> Args { get; set; }

        public IReadOnlyList<PostActionInstruction> ManualInstructions { get; set; }

        // TODO: read and deserialize this?
        public string ConfigFile { get; set; }
    }
}
