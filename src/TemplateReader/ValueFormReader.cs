using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateData.ValueForms;
using TemplateReader.Utils;

namespace TemplateReader
{
    public static class ValueFormReader
    {
        public static IReadOnlyDictionary<string, ValueFormData> ReadValueForms(JObject source)
        {
            Dictionary<string, ValueFormData> valueFormsData = new Dictionary<string, ValueFormData>();

            IReadOnlyDictionary<string, JToken> formsSource = source.ToJTokenDictionary(StringComparer.OrdinalIgnoreCase, nameof(TemplateData.TemplateData.Forms));

            foreach (KeyValuePair<string, JToken> formNameAndData in formsSource)
            {
                string identifier = formNameAndData.Value.ToString(nameof(ValueFormData.Identifier));
                string name = formNameAndData.Value.ToString(nameof(ValueFormData.Name));

                if (identifier == "chain")
                {
                    ChainValueFormData formData = new ChainValueFormData()
                    {
                        Identifier = identifier,
                        Name = name,
                        Steps = formNameAndData.Value.ArrayAsStrings(nameof(ChainValueFormData.Steps))
                    };
                    valueFormsData[name] = formData;
                }
                else if (identifier == "replace")
                {
                    string pattern = formNameAndData.Value.ToString(nameof(ReplacementValueFormData.Pattern));
                    ReplacementValueFormData formData = new ReplacementValueFormData()
                    {
                        Identifier = identifier,
                        Name = name,
                        Pattern = formNameAndData.Value.ToString(nameof(ReplacementValueFormData.Pattern)),
                        Replacement = formNameAndData.Value.ToString(nameof(ReplacementValueFormData.Replacement))
                    };
                    valueFormsData[name] = formData;
                }
                else
                {
                    ValueFormData formData = new ValueFormData()
                    {
                        Identifier = identifier,
                        Name = name
                    };
                    valueFormsData[name] = formData;
                }
            }

            return valueFormsData;
        }
    }
}
