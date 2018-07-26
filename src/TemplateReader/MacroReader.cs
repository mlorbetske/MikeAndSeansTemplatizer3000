using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using TemplateData.Macros;
using TemplateData.Symbols;
using TemplateReader.Utils;

namespace TemplateReader
{
    public static class MacroReader
    {
        public static BaseMacroData ReadMacro(JObject symbol)
        {
            JObject paramsObject = (JObject)symbol.Get<JToken>("parameters");
            string generator = symbol.ToString(nameof(GeneratedSymbolData.Generator));
            BaseMacroData macro = null;

            switch (generator)
            {
                case CaseChangeMacroData.Identity:
                    macro = ReadCaseChangeMacroData(paramsObject);
                    break;
                case CoalesceMacroData.Identity:
                    macro = ReadCoalesceMacroData(paramsObject);
                    break;
                case ConstantMacroData.Identity:
                    macro = ReadConstantMacroData(paramsObject);
                    break;
                case EvaluateMacroData.Identity:
                    macro = ReadEvaluateMacroData(paramsObject);
                    break;
                case GeneratePortNumberMacroData.Identity:
                    macro = ReadGeneratePortNumberMacroData(paramsObject);
                    break;
                case GuidMacroData.Identity:
                    macro = ReadGuidMacroData(paramsObject);
                    break;
                case NowMacroData.Identity:
                    macro = ReadNowMacroData(paramsObject);
                    break;
                case ProcessValueFormMacroData.Identity:
                    macro = ReadProcessValueFormMacroData(paramsObject);
                    break;
                case RandomMacroData.Identity:
                    macro = ReadRandomMacroData(paramsObject);
                    break;
                case RegexMacroData.Identity:
                    macro = ReadRegexMacroData(paramsObject);
                    break;
                case RegexMatchMacroData.Identity:
                    macro = ReadRegexMatchMacroData(paramsObject);
                    break;
                case SwitchMacroData.Identity:
                    macro = ReadSwitchMacroData(paramsObject);
                    break;
                default:
                    macro = ReadUnsupportedTypeMacroData(paramsObject, generator);
                    break;
            }

            return macro;
        }

        public static CaseChangeMacroData ReadCaseChangeMacroData(JObject parameters)
        {
            CaseChangeMacroData macroData = new CaseChangeMacroData()
            {
                SourceVariable = parameters.ToString(nameof(CaseChangeMacroData.SourceVariable)),
                ToLower = parameters.ToBool(nameof(CaseChangeMacroData.ToLower))
            };

            return macroData;
        }

        public static CoalesceMacroData ReadCoalesceMacroData(JObject parameters)
        {
            CoalesceMacroData macroData = new CoalesceMacroData()
            {
                SourceVariableName = parameters.ToString(nameof(CoalesceMacroData.SourceVariableName)),
                DefaultValue = parameters.ToString(nameof(CoalesceMacroData.DefaultValue)),
                FallbackVariableName = parameters.ToString(nameof(CoalesceMacroData.FallbackVariableName))
            };

            return macroData;
        }

        public static ConstantMacroData ReadConstantMacroData(JObject parameters)
        {
            ConstantMacroData macroData = new ConstantMacroData()
            {
                Value = parameters.ToString(nameof(ConstantMacroData.Value))
            };

            return macroData;
        }

        public static EvaluateMacroData ReadEvaluateMacroData(JObject parameters)
        {
            EvaluateMacroData macroData = new EvaluateMacroData()
            {
                Value = parameters.ToString(nameof(EvaluateMacroData.Value)),
                Evaluator = parameters.ToString(nameof(EvaluateMacroData.Evaluator))
            };

            return macroData;
        }

        public static GeneratePortNumberMacroData ReadGeneratePortNumberMacroData(JObject parameters)
        {
            GeneratePortNumberMacroData macroData = new GeneratePortNumberMacroData()
            {
                Low = parameters.ToInt32(nameof(GeneratePortNumberMacroData.Low)),
                High = parameters.ToInt32(nameof(GeneratePortNumberMacroData.High))
            };

            return macroData;
        }

        public static GuidMacroData ReadGuidMacroData(JObject parameters)
        {
            GuidMacroData macroData = new GuidMacroData()
            {
                Format = parameters.ToString(nameof(GuidMacroData.Format))
            };

            return macroData;
        }

        public static NowMacroData ReadNowMacroData(JObject parameters)
        {
            NowMacroData macroData = new NowMacroData()
            {
                Format = parameters.ToString(nameof(NowMacroData.Format)),
                Utc = parameters.ToBool(nameof(NowMacroData.Utc))
            };

            return macroData;
        }

        public static ProcessValueFormMacroData ReadProcessValueFormMacroData(JObject parameters)
        {
            ProcessValueFormMacroData macroData = new ProcessValueFormMacroData()
            {
                SourceVariable = parameters.ToString(nameof(ProcessValueFormMacroData.SourceVariable)),
                FormName = parameters.ToString(nameof(ProcessValueFormMacroData.FormName))
            };

            return macroData;
        }

        public static RandomMacroData ReadRandomMacroData(JObject parameters)
        {
            RandomMacroData macroData = new RandomMacroData()
            {
                Low = parameters.ToInt32(nameof(RandomMacroData.Low)),
                High = parameters.ToInt32(nameof(RandomMacroData.High))
            };

            return macroData;
        }

        public static RegexMacroData ReadRegexMacroData(JObject parameters)
        {
            List<RegexMacroStepData> stepList = new List<RegexMacroStepData>();

            JToken stepsParam = parameters.Get<JToken>(nameof(RegexMacroData.Steps));
            foreach (JToken entry in (JArray)stepsParam)
            {
                JObject map = (JObject)entry;
                RegexMacroStepData step = new RegexMacroStepData()
                {
                    Regex = map.ToString(nameof(RegexMacroStepData.Regex)),
                    Replacement = map.ToString(nameof(RegexMacroStepData.Replacement))
                };
                stepList.Add(step);
            }

            RegexMacroData macroData = new RegexMacroData()
            {
                SourceVariable = parameters.ToString(nameof(RegexMacroData.SourceVariable)),
                Steps = stepList
            };

            return macroData;
        }

        public static RegexMatchMacroData ReadRegexMatchMacroData(JObject parameters)
        {
            RegexMatchMacroData macroData = new RegexMatchMacroData()
            {
                SourceVariable = parameters.ToString(nameof(RegexMatchMacroData.SourceVariable)),
                Pattern = parameters.ToString(nameof(RegexMatchMacroData.Pattern))
            };

            return macroData;
        }

        public static SwitchMacroData ReadSwitchMacroData(JObject parameters)
        {
            List<SwitchMacroSwitchesData> switchList = new List<SwitchMacroSwitchesData>();

            JToken switchesParam = parameters.Get<JToken>(nameof(SwitchMacroData.Switches));
            foreach (JToken entry in (JArray)switchesParam)
            {
                JObject map = (JObject)entry;
                SwitchMacroSwitchesData switchEntry = new SwitchMacroSwitchesData()
                {
                    Condition = map.ToString(nameof(SwitchMacroSwitchesData.Condition)),
                    Value = map.ToString(nameof(SwitchMacroSwitchesData.Value))
                };
                switchList.Add(switchEntry);
            }

            SwitchMacroData macroData = new SwitchMacroData()
            {
                Switches = switchList
            };

            return macroData;
        }

        public static UnsupportedTypeMacroData ReadUnsupportedTypeMacroData(JObject parameters, string generator)
        {
            UnsupportedTypeMacroData macroData = new UnsupportedTypeMacroData()
            {
                SpecifiedType = generator,
                RawConfig = parameters.ToString()
            };

            return macroData;
        }
    }
}
