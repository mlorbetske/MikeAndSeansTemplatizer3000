using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateData.ValueForms
{
    public class ReplacementValueFormData : ValueFormData
    {
        // The regex to match
        public string Pattern { get; set; }

        public string Replacement { get; set; }
    }
}
