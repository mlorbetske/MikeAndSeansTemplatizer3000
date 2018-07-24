using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TemplateData.Macros;
using TemplateData.Symbols;
using TemplateReader.Utils;

namespace TemplateReader
{
    public static class MacroReader
    {
        // reads macros on a generated symbol
        public static IReadOnlyDictionary<string, BaseMacroData> ReadMacros(JObject symbol)
        {
            IReadOnlyDictionary<string, JToken> rawParameters = symbol.ToJTokenDictionary(StringComparer.Ordinal, nameof(GeneratedSymbolData.Parameters));
            Dictionary<string, BaseMacroData> macros = new Dictionary<string, BaseMacroData>();

            foreach (KeyValuePair<string, JToken> paramNameAndInfo in rawParameters)
            {
                string macroType = paramNameAndInfo.Value.ToString(nameof(BaseMacroData.Type));
                string variableName = paramNameAndInfo.Key;

                switch (macroType)
                {
                    case CaseChangeMacroData.Identity:
                        macros[variableName] = ReadCaseChangeMacroData(paramNameAndInfo.Value, variableName);
                        break;
                    case CoalesceMacroData.Identity:
                        macros[variableName] = ReadCoalesceMacroData(paramNameAndInfo.Value, variableName);
                        break;
                    case ConstantMacroData.Identity:
                        macros[variableName] = ReadConstantMacroData(paramNameAndInfo.Value, variableName);
                        break;
                    case EvaluateMacroData.Identity:
                        macros[variableName] = ReadEvaluateMacroData(paramNameAndInfo.Value, variableName);
                        break;
                    case GeneratePortNumberMacroData.Identity:
                        macros[variableName] = ReadGeneratePortNumberMacroData(paramNameAndInfo.Value, variableName);
                        break;
                    case GuidMacroData.Identity:
                        macros[variableName] = ReadGuidMacroData(paramNameAndInfo.Value, variableName);
                        break;
                    case NowMacroData.Identity:
                        macros[variableName] = ReadNowMacroData(paramNameAndInfo.Value, variableName);
                        break;
                    case ProcessValueFormMacroData.Identity:
                        macros[variableName] = ReadProcessValueFormMacroData(paramNameAndInfo.Value, variableName);
                        break;
                    case RandomMacroData.Identity:
                        macros[variableName] = ReadRandomMacroData(paramNameAndInfo.Value, variableName);
                        break;
                    case RegexMacroData.Identity:
                        macros[variableName] = ReadRegexMacroData(paramNameAndInfo.Value, variableName);
                        break;
                    case RegexMatchMacroData.Identity:
                        macros[variableName] = ReadRegexMatchMacroData(paramNameAndInfo.Value, variableName);
                        break;
                    case SwitchMacroData.Identity:
                        macros[variableName] = ReadSwitchMacroData(paramNameAndInfo.Value, variableName);
                        break;
                    default:
                        macros[variableName] = ReadUnsupportedTypeMacroData(paramNameAndInfo.Value, variableName);
                        break;
                }
            }

            return macros;
        }

        public static CaseChangeMacroData ReadCaseChangeMacroData(JToken sourceToken, string variableName)
        {
            return new CaseChangeMacroData()
            {
                VariableName = variableName,
                DataType = sourceToken.ToString(nameof(BaseMacroData.DataType)),
                SourceVariable = sourceToken.ToString("source"),
                ToLower = sourceToken.ToBool(nameof(CaseChangeMacroData.ToLower)),
            };
        }

        public static CoalesceMacroData ReadCoalesceMacroData(JToken sourceToken, string variableName)
        {
            return new CoalesceMacroData()
            {
                VariableName = variableName,
                DataType = sourceToken.ToString(nameof(BaseMacroData.DataType)),
                SourceVariableName = sourceToken.ToString(nameof(CoalesceMacroData.SourceVariableName)),
                DefaultValue = sourceToken.ToString(nameof(CoalesceMacroData.DefaultValue)),
                FallbackVariableName = sourceToken.ToString(nameof(CoalesceMacroData.FallbackVariableName))
            };
        }

        public static ConstantMacroData ReadConstantMacroData(JToken sourceToken, string variableName)
        {
            return new ConstantMacroData()
            {
                VariableName = variableName,
                DataType = sourceToken.ToString(nameof(BaseMacroData.DataType)),
                Value = sourceToken.ToString(nameof(ConstantMacroData.Value))
            };
        }

        public static EvaluateMacroData ReadEvaluateMacroData(JToken sourceToken, string variableName)
        {
            return new EvaluateMacroData()
            {
                VariableName = variableName,
                DataType = sourceToken.ToString(nameof(BaseMacroData.DataType)),
                Value = sourceToken.ToString(nameof(EvaluateMacroData.Value)),
                Evaluator = sourceToken.ToString(nameof(EvaluateMacroData.Evaluator))
            };
        }

        public static GeneratePortNumberMacroData ReadGeneratePortNumberMacroData(JToken sourceToken, string variableName)
        {
            return new GeneratePortNumberMacroData()
            {
                VariableName = variableName,
                DataType = sourceToken.ToString(nameof(BaseMacroData.DataType)),
                Low = sourceToken.ToInt32(nameof(GeneratePortNumberMacroData.Low)),
                High = sourceToken.ToInt32(nameof(GeneratePortNumberMacroData.High))
            };
        }

        public static GuidMacroData ReadGuidMacroData(JToken sourceToken, string variableName)
        {
            return new GuidMacroData()
            {
                VariableName = variableName,
                DataType = sourceToken.ToString(nameof(BaseMacroData.DataType)),
                Format = sourceToken.ToString(nameof(GuidMacroData.Format))
            };
        }

        public static NowMacroData ReadNowMacroData(JToken sourceToken, string variableName)
        {
            return new NowMacroData()
            {
                VariableName = variableName,
                DataType = sourceToken.ToString(nameof(BaseMacroData.DataType)),
                Format = sourceToken.ToString(nameof(NowMacroData.Format)),
                Utc = sourceToken.ToBool(nameof(NowMacroData.Utc))
            };
        }

        public static ProcessValueFormMacroData ReadProcessValueFormMacroData(JToken sourceToken, string variableName)
        {
            return new ProcessValueFormMacroData()
            {
                VariableName = variableName,
                DataType = sourceToken.ToString(nameof(BaseMacroData.DataType)),
                SourceVariable = sourceToken.ToString(nameof(ProcessValueFormMacroData.SourceVariable)),
                FormName = sourceToken.ToString(nameof(ProcessValueFormMacroData.FormName)),
            };
        }

        public static RandomMacroData ReadRandomMacroData(JToken sourceToken, string variableName)
        {
            return new RandomMacroData()
            {
                VariableName = variableName,
                DataType = sourceToken.ToString(nameof(BaseMacroData.DataType)),
                Low = sourceToken.ToInt32(nameof(RandomMacroData.Low)),
                High = sourceToken.ToInt32(nameof(RandomMacroData.High))
            };
        }

        public static RegexMacroData ReadRegexMacroData(JToken sourceToken, string variableName)
        {
            return new RegexMacroData()
            {
                VariableName = variableName,
                DataType = sourceToken.ToString(nameof(BaseMacroData.DataType)),
                SourceVariable = sourceToken.ToString(nameof(RegexMacroData.SourceVariable)),
                Steps = sourceToken.ToStringStringKeyValuePairList(nameof(RegexMacroData.Steps)),
            };
        }

        public static RegexMatchMacroData ReadRegexMatchMacroData(JToken sourceToken, string variableName)
        {
            return new RegexMatchMacroData()
            {
                VariableName = variableName,
                DataType = sourceToken.ToString(nameof(BaseMacroData.DataType)),
                SourceVariable = sourceToken.ToString(nameof(RegexMatchMacroData.SourceVariable)),
                Pattern = sourceToken.ToString(nameof(RegexMatchMacroData.Pattern))
            };
        }

        public static SwitchMacroData ReadSwitchMacroData(JToken sourceToken, string variableName)
        {
            return new SwitchMacroData()
            {
                VariableName = variableName,
                DataType = sourceToken.ToString(nameof(BaseMacroData.DataType)),
                Switches = sourceToken.ToStringStringKeyValuePairList(nameof(SwitchMacroData.Switches))
            };
        }

        public static UnsupportedTypeMacroData ReadUnsupportedTypeMacroData(JToken sourceToken, string variableName)
        {
            return new UnsupportedTypeMacroData()
            {
                VariableName = variableName,
                DataType = sourceToken.ToString(nameof(BaseMacroData.DataType)),
                SpecifiedType = sourceToken.ToString(nameof(BaseMacroData.Type)),
                RawConfig = sourceToken.ToString()
            };
        }
    }
}
