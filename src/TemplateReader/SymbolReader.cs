using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TemplateData.Symbols;
using TemplateReader.Utils;

namespace TemplateReader
{
    public static class SymbolReader
    {
        internal const string BindSymbolTypeName = "bind";

        public static IReadOnlyDictionary<string, SymbolData> ReadSymbols(JObject source)
        {
            Dictionary<string, SymbolData> allSymbols = new Dictionary<string, SymbolData>();

            foreach (JProperty property in source.PropertiesOf(nameof(TemplateData.TemplateData.Symbols)))
            {
                if (!(property.Value is JObject symbol))
                {
                    continue;
                }

                string type = symbol.ToString(nameof(SymbolData.Type));

                switch (type)
                {
                    case ComputedSymbolData.TypeName:
                        allSymbols[property.Name] = ReadComputedSymbol(symbol);
                        break;
                    case BindSymbolTypeName:
                    case GeneratedSymbolData.TypeName:
                        allSymbols[property.Name] = ReadGeneratedSymbol(symbol);
                        break;
                    case DerivedSymbolData.TypeName:
                        allSymbols[property.Name] = ReadDerivedSymbol(symbol);
                        break;
                    case ParameterSymbolData.TypeName:
                        allSymbols[property.Name] = ReadParameterSymbol(symbol);
                        break;
                }
            }

            return allSymbols;
        }

        private static ComputedSymbolData ReadComputedSymbol(JObject symbol)
        {
            return new ComputedSymbolData()
            {
                Type = symbol.ToString(nameof(ComputedSymbolData.Type)),
                DataType = symbol.ToString(nameof(ComputedSymbolData.DataType)),
                Value = symbol.ToString(nameof(ComputedSymbolData.Value)),
                Evaluator = symbol.ToString(nameof(ComputedSymbolData.Evaluator))
            };
        }

        private static GeneratedSymbolData ReadGeneratedSymbol(JObject symbol)
        {
            GeneratedSymbolData symbolData = new GeneratedSymbolData()
            {
                Type = symbol.ToString(nameof(GeneratedSymbolData.Type)),
                DataType = symbol.ToString(nameof(GeneratedSymbolData.DataType)),
                Binding = symbol.ToString(nameof(GeneratedSymbolData.Binding)),
                Replaces = symbol.ToString(nameof(GeneratedSymbolData.Replaces)),
                Generator = symbol.ToString(nameof(GeneratedSymbolData.Generator)),
                ReplacementContexts = ReadReplacementContexts(symbol)
            };

            symbolData.Parameters = MacroReader.ReadMacros(symbol);

            return symbolData;
        }

        private static DerivedSymbolData ReadDerivedSymbol(JObject symbol)
        {
            DerivedSymbolData symbolData = new DerivedSymbolData()
            {
                ValueTransform = symbol.ToString(nameof(DerivedSymbolData.ValueTransform)),
                ValueSource = symbol.ToString(nameof(DerivedSymbolData.ValueSource))
            };

            ReadBaseValueSymbolData(symbol, symbolData);
            return symbolData;
        }

        private static ParameterSymbolData ReadParameterSymbol(JObject symbol)
        {
            ParameterSymbolData symbolData = new ParameterSymbolData()
            {
                DefaultIfOptionWithoutValue = symbol.ToString(nameof(ParameterSymbolData.DefaultIfOptionWithoutValue))
            };

            ReadBaseValueSymbolData(symbol, symbolData);

            Dictionary<string, string> choicesAndDescriptions = new Dictionary<string, string>();
            symbolData.ChoicesAndDescriptions = choicesAndDescriptions;

            if (String.Equals(symbolData.DataType, "choice", StringComparison.OrdinalIgnoreCase))
            {
                foreach (JObject choiceObject in symbol.Items<JObject>("Choices"))
                {
                    string choice = choiceObject.ToString("choice");
                    string description = choiceObject.ToString("description");
                    // TODO: consider allowing last-in-wins, in case 2 choices are the same.
                    choicesAndDescriptions.Add(choice, description ?? string.Empty);
                }
            }
            else if (string.Equals(symbolData.DataType, "bool", StringComparison.OrdinalIgnoreCase)
                && string.IsNullOrEmpty(symbolData.DefaultIfOptionWithoutValue))
            {
                // bool flags are considred true if they're provided without a value.
                symbolData.DefaultIfOptionWithoutValue = "true";
            }

            return symbolData;
        }

        private static void ReadBaseValueSymbolData(JObject symbol, BaseValueSymbolData symbolData)
        {
            symbolData.Type = symbol.ToString(nameof(BaseValueSymbolData.Type));
            symbolData.Description = symbol.ToString(nameof(BaseValueSymbolData.Description));
            symbolData.DefaultValue = symbol.ToString(nameof(BaseValueSymbolData.DefaultValue));
            symbolData.Forms = ReadFormsForValueSymbol(symbol);
            symbolData.IsRequired = symbol.ToBool(nameof(BaseValueSymbolData.IsRequired));
            symbolData.DataType = symbol.ToString(nameof(BaseValueSymbolData.DataType));
            symbolData.FileRename = symbol.ToString(nameof(BaseValueSymbolData.FileRename));
            symbolData.Binding = symbol.ToString(nameof(BaseValueSymbolData.Binding));
            symbolData.Replaces = symbol.ToString(nameof(BaseValueSymbolData.Replaces));
            symbolData.ReplacementContexts = ReadReplacementContexts(symbol);
        }

        private static SymbolValueFormData ReadFormsForValueSymbol(JObject symbol)
        {
            if (!symbol.TryGetValue(nameof(BaseValueSymbolData.Forms), StringComparison.OrdinalIgnoreCase, out JToken formsToken) 
                    || !(formsToken is JObject formsObject))
            {
                return new SymbolValueFormData()
                {
                    GlobalFormNames = new List<string>(),
                    AddIdentityForm = true
                };
            }

            JToken globalConfig = symbol.Property("global");
            IReadOnlyList<string> globalFormNames;
            bool addIdentity;

            if (globalConfig.Type == JTokenType.Array)
            {
                // config is just an array of form names - the simpler way of defining them.
                globalFormNames = globalConfig.ArrayAsStrings().ToList();
                addIdentity = true;
            }
            else if (globalConfig.Type == JTokenType.Object)
            {
                globalFormNames = globalConfig.ArrayAsStrings("forms").ToList();
                addIdentity = globalConfig.ToBool("addIdentity", true);
            }
            else
            {
                // TODO: cause this to propagate the symbol name
                throw new Exception("Malformed global value forms.");
            }

            return new SymbolValueFormData()
            {
                GlobalFormNames = globalFormNames,
                AddIdentityForm = addIdentity
            };
        }

        private static IReadOnlyList<ReplacementContextData> ReadReplacementContexts(JObject symbol)
        {
            JArray onlyIf = symbol.Get<JArray>("onlyIf");

            if (onlyIf != null)
            {
                List<ReplacementContextData> contexts = new List<ReplacementContextData>();
                foreach (JToken entry in onlyIf.Children())
                {
                    if (!(entry is JObject x))
                    {
                        continue;
                    }

                    string before = entry.ToString("before");
                    string after = entry.ToString("after");
                    contexts.Add(new ReplacementContextData(before, after));
                }

                return contexts;
            }
            else
            {
                return Empty<ReplacementContextData>.List.Value;
            }
        }
    }
}
