using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateData.CustomOperations.OperationProviders
{
    public class FlagOperationData : OperationBaseData
    {
        public const string OperationName = "flags";

        public string Flag { get; set; }
        public string On { get; set; }
        public string Off { get; set; }
        public string OnNoEmit { get; set; }
        public string OffNoEmit { get; set; }
        public bool Default { get; set; }
    }
}
