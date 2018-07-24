using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateData.Symbols
{
    public class SymbolValueFormData
    {
        public IReadOnlyList<string> GlobalFormNames { get; set; }

        // true if the identity form should be implicitly included in this list
        public bool AddIdentityForm { get; set; }
    }
}
