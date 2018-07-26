using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
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
                string name = formNameAndData.Key;

                if (string.Equals(identifier, ChainValueFormData.FormName, StringComparison.OrdinalIgnoreCase))
                {
                    ChainValueFormData formData = new ChainValueFormData()
                    {
                        Identifier = identifier,
                        Name = name,
                        Steps = formNameAndData.Value.ArrayAsStrings(nameof(ChainValueFormData.Steps))
                    };
                    valueFormsData[name] = formData;
                }
                else if (string.Equals(identifier, ReplacementValueFormData.FormName, StringComparison.OrdinalIgnoreCase))
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
                    UnsupportedTypeValueFormData formData = new UnsupportedTypeValueFormData()
                    {
                        Identifier = identifier,
                        Name = name,
                        RawConfig = formNameAndData.Value.ToString()
                    };
                    valueFormsData[name] = formData;
                }
            }

            return valueFormsData;
        }
    }
}
