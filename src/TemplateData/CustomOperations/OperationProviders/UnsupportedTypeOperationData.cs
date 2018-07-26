using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateData.CustomOperations.OperationProviders
{
    public class UnsupportedTypeOperationData : OperationBaseData
    {
        public string SpecifiedType { get;set; }
        public string RawConfig { get; set; }
    }
}
