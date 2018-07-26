using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TemplateData.CustomOperations;
using TemplateData.CustomOperations.OperationProviders;
using TemplateReader.Utils;

namespace TemplateReader
{
    public static class CustomFileGlobReader
    {
        public static IReadOnlyList<CustomFileGlobData> ReadGlobSpecificCustomOperations(JObject source)
        {
            List<CustomFileGlobData> globSpecificCustomOperations = new List<CustomFileGlobData>();
            IReadOnlyDictionary<string, JToken> allSpecialOpsConfig = source.ToJTokenDictionary(StringComparer.OrdinalIgnoreCase, "SpecialCustomOperations");

            foreach (KeyValuePair<string, JToken> globConfigKeyValue in allSpecialOpsConfig)
            {
                string globName = globConfigKeyValue.Key;
                JObject globConfig = (JObject)globConfigKeyValue.Value;
                CustomFileGlobData globData = ReadCustomFileGlobData(globConfig, globName);
                globSpecificCustomOperations.Add(globData);
            }

            return globSpecificCustomOperations;
        }

        public static CustomFileGlobData ReadCustomFileGlobData(JObject source, string globName)
        {
            CustomFileGlobData fileGlobData = new CustomFileGlobData()
            {
                Glob = globName,
                Operations = ReadCustomOperationsList(source),
                VariableFormat = ReadVariableConfigData(source),
                FlagPrefix = source.ToString(nameof(CustomFileGlobData.FlagPrefix))
            };

            return fileGlobData;
        }

        private static VariableConfigData ReadVariableConfigData(JObject variableSource)
        {
            if (variableSource == null || !variableSource.TryGetValue(nameof(CustomFileGlobData.VariableFormat), StringComparison.OrdinalIgnoreCase, out JToken variableDataJToken))
            {
                return null;
            }

            JObject variableData = (JObject)variableDataJToken;
            List<VariableConfigSource> variableConfigSources = new List<VariableConfigSource>();

            if (variableData.TryGetValue(nameof(VariableConfigData.Sources), StringComparison.OrdinalIgnoreCase, out JToken sourcesData))
            {
                foreach (JObject sourceConfig in (JArray)sourcesData)
                {
                    VariableConfigSource source = new VariableConfigSource()
                    {
                        Name = sourceConfig.ToString(nameof(VariableConfigSource.Name)),
                        Format = sourceConfig.ToString(nameof(VariableConfigSource.Format))
                    };
                    variableConfigSources.Add(source);
                }
            }

            VariableConfigData variableConfig = new VariableConfigData()
            {
                Sources = variableConfigSources,
                FallbackFormat = variableSource.ToString(nameof(VariableConfigData.FallbackFormat)),
                Expand = variableSource.ToBool(nameof(VariableConfigData.Expand))
            };

            return variableConfig;
        }

        private static IReadOnlyList<CustomOperationData> ReadCustomOperationsList(JObject source)
        {
            List<CustomOperationData> customOperations = new List<CustomOperationData>();
            JToken operationSourceList = null;

            if (source != null)
            {
                operationSourceList = source.Get<JToken>(nameof(CustomFileGlobData.Operations));
            }

            if (operationSourceList == null)
            {
                return customOperations;
            }

            foreach (JObject operationSource in (JArray)operationSourceList)
            {
                CustomOperationData operation = ReadCustomOperation(operationSource);
                customOperations.Add(operation);
            }

            return customOperations;
        }

        private static CustomOperationData ReadCustomOperation(JObject operationSource)
        {
            string type = operationSource.ToString(nameof(CustomOperationData.Type));
            OperationBaseData operation = null;
            JObject operationConfig = (JObject)operationSource.Get<JToken>("configuration");

            switch (type)
            {
                case BalancedNestingOperationData.OperationName:
                    operation = ReadBalancedNestingData(operationConfig);
                    break;
                case ConditionalCustomOperationData.OperationName:
                    operation = ReadConditionalData(operationConfig);
                    break;
                case FlagOperationData.OperationName:
                    operation = ReadFlagOperationData(operationConfig);
                    break;
                case IncludeOperationData.OperationName:
                    operation = ReadIncludeOperationData(operationConfig);
                    break;
                case RegionOperationData.OperationName:
                    operation = ReadRegionOperationData(operationConfig);
                    break;
                case ReplacementOperationData.OperationName:
                    operation = ReadReplacementOperationData(operationConfig);
                    break;
                default:
                    operation = ReadUnsupportedTypeOperationData(operationConfig);
                    break;
            }

            CustomOperationData operationData = new CustomOperationData()
            {
                Type = type,
                Condition = operationSource.ToString(nameof(CustomOperationData.Condition)),
                Operation = operation
            };

            return operationData;
        }

        private static BalancedNestingOperationData ReadBalancedNestingData(JObject operationConfig)
        {
            return new BalancedNestingOperationData()
            {
                StartToken = operationConfig.ToString(nameof(BalancedNestingOperationData.StartToken)),
                RealEndToken = operationConfig.ToString(nameof(BalancedNestingOperationData.RealEndToken)),
                PseudoEndToken = operationConfig.ToString(nameof(BalancedNestingOperationData.PseudoEndToken)),
                Id = operationConfig.ToString(nameof(BalancedNestingOperationData.Id)),
                ResetFlag = operationConfig.ToString(nameof(BalancedNestingOperationData.ResetFlag)),
                OnByDefault = operationConfig.ToBool(nameof(BalancedNestingOperationData.OnByDefault))
            };
        }

        private static OperationBaseData ReadConditionalData(JObject operationConfig)
        {
            string style = operationConfig.ToString(nameof(ConditionalCustomOperationData.Style));
            if (string.IsNullOrEmpty(style))
            {
                // custom is the default style, if style isn't specified, custom is assumed.
                style = ConditionalCustomOperationData.Style;
            }

            OperationBaseData operation = null;

            switch (style)
            {
                case ConditionalCustomOperationData.Style:
                    operation = ReadCustomConditionalData(operationConfig);
                    break;
                case ConditionalLineCommentOperationData.Style:
                    operation = ReadConditionalLineCommentData(operationConfig);
                    break;
                case ConditionalBlockCommentOperationData.Style:
                    operation = ReadConditionalBlockCommentData(operationConfig);
                    break;
                default:
                    // for unsupported styles
                    operation = ReadUnsupportedTypeOperationData(operationConfig);
                    break;
            }

            return operation;
        }

        private static ConditionalCustomOperationData ReadCustomConditionalData(JObject operationConfig)
        {
            ConditionalCustomOperationData customData = new ConditionalCustomOperationData()
            {
                Id = operationConfig.ToString(nameof(ConditionalCustomOperationData.Id)),
                OnByDefault = operationConfig.ToBool(nameof(ConditionalCustomOperationData.OnByDefault)),
                IfTokens = operationConfig.ArrayAsStrings("if", defaultAsNull: true),
                ElseTokens = operationConfig.ArrayAsStrings("else", defaultAsNull: true),
                ElseIfTokens = operationConfig.ArrayAsStrings("elseif", defaultAsNull: true),
                ActionableIfTokens = operationConfig.ArrayAsStrings("actionableIf", defaultAsNull: true),
                ActionableElseTokens = operationConfig.ArrayAsStrings("actionableElse", defaultAsNull: true),
                ActionableElseIfTokens = operationConfig.ArrayAsStrings("actionableElseif", defaultAsNull: true),
                Actions = operationConfig.ArrayAsStrings(nameof(ConditionalCustomOperationData.Actions), defaultAsNull: true),
                EndIfTokens = operationConfig.ArrayAsStrings("endif", defaultAsNull: true),
                Evaluator = operationConfig.ToString(nameof(ConditionalCustomOperationData.Evaluator)),
                Trim = operationConfig.ToBool(nameof(ConditionalCustomOperationData.Trim)),
                WholeLine = operationConfig.ToBool(nameof(ConditionalCustomOperationData.WholeLine))
            };

            return customData;
        }

        private static ConditionalLineCommentOperationData ReadConditionalLineCommentData(JObject operationConfig)
        {
            ConditionalLineCommentOperationData lineCommentData = new ConditionalLineCommentOperationData()
            {
                Id = operationConfig.ToString(nameof(ConditionalLineCommentOperationData.Id)),
                OnByDefault = operationConfig.ToBool(nameof(ConditionalLineCommentOperationData.OnByDefault)),
                Token = operationConfig.ToString(nameof(ConditionalLineCommentOperationData.Token)),
                IfKeywords = operationConfig.Get<JToken>(nameof(ConditionalLineCommentOperationData.IfKeywords))
                                        .JTokenStringOrArrayToCollection(null),
                ElseIfKeywords = operationConfig.Get<JToken>(nameof(ConditionalLineCommentOperationData.ElseIfKeywords))
                                        .JTokenStringOrArrayToCollection(null),
                ElseKeyworkds = operationConfig.Get<JToken>(nameof(ConditionalLineCommentOperationData.ElseKeyworkds))
                                        .JTokenStringOrArrayToCollection(null),
                EndIfKeywords = operationConfig.Get<JToken>(nameof(ConditionalLineCommentOperationData.EndIfKeywords))
                                        .JTokenStringOrArrayToCollection(null),
                KeywordPrefix = operationConfig.ToString(nameof(ConditionalLineCommentOperationData.KeywordPrefix)),
                Evaluator = operationConfig.ToString(nameof(ConditionalLineCommentOperationData.Evaluator)),
                Trim = operationConfig.ToBool(nameof(ConditionalLineCommentOperationData.Trim)),
                WholeLine = operationConfig.ToBool(nameof(ConditionalLineCommentOperationData.WholeLine))
            };

            return lineCommentData;
        }

        private static ConditionalBlockCommentOperationData ReadConditionalBlockCommentData(JObject operationConfig)
        {
            ConditionalBlockCommentOperationData blockCommentData = new ConditionalBlockCommentOperationData()
            {
                Id = operationConfig.ToString(nameof(ConditionalBlockCommentOperationData.Id)),
                OnByDefault = operationConfig.ToBool(nameof(ConditionalBlockCommentOperationData.OnByDefault)),
                StartToken = operationConfig.Get<JToken>(nameof(ConditionalBlockCommentOperationData.StartToken))
                                    .JTokenStringOrArrayToCollection(null),
                EndToken = operationConfig.Get<JToken>(nameof(ConditionalBlockCommentOperationData.EndToken))
                                    .JTokenStringOrArrayToCollection(null),
                PseudoEndToken = operationConfig.Get<JToken>(nameof(ConditionalBlockCommentOperationData.PseudoEndToken))
                                    .JTokenStringOrArrayToCollection(null),
                KeywordPrefix = operationConfig.ToString(nameof(ConditionalBlockCommentOperationData.KeywordPrefix)),
                Evaluator = operationConfig.ToString(nameof(ConditionalBlockCommentOperationData.Evaluator)),
                Trim = operationConfig.ToBool(nameof(ConditionalBlockCommentOperationData.Trim)),
                WholeLine = operationConfig.ToBool(nameof(ConditionalBlockCommentOperationData.WholeLine))
            };

            return blockCommentData;
        }

        private static FlagOperationData ReadFlagOperationData(JObject operationConfig)
        {
            FlagOperationData flagOperationData = new FlagOperationData()
            {
                Id = operationConfig.ToString(nameof(FlagOperationData.Id)),
                OnByDefault = operationConfig.ToBool(nameof(FlagOperationData.OnByDefault)),
                Flag = operationConfig.ToString(nameof(FlagOperationData.Flag)),
                On = operationConfig.ToString(nameof(FlagOperationData.On)),
                Off = operationConfig.ToString(nameof(FlagOperationData.Off)),
                OnNoEmit = operationConfig.ToString(nameof(FlagOperationData.OnNoEmit)),
                OffNoEmit = operationConfig.ToString(nameof(FlagOperationData.OffNoEmit)),
                Default = operationConfig.ToBool(nameof(FlagOperationData.Default))
            };

            return flagOperationData;
        }

        private static IncludeOperationData ReadIncludeOperationData(JObject operationConfig)
        {
            IncludeOperationData includeOperationData = new IncludeOperationData()
            {
                Id = operationConfig.ToString(nameof(IncludeOperationData.Id)),
                OnByDefault = operationConfig.ToBool(nameof(IncludeOperationData.OnByDefault)),
                Start = operationConfig.ToString(nameof(IncludeOperationData.Start)),
                End = operationConfig.ToString(nameof(IncludeOperationData.End))
            };

            return includeOperationData;
        }

        private static RegionOperationData ReadRegionOperationData(JObject operationConfig)
        {
            RegionOperationData regionOperationData = new RegionOperationData()
            {
                Id = operationConfig.ToString(nameof(RegionOperationData.Id)),
                OnByDefault = operationConfig.ToBool(nameof(RegionOperationData.OnByDefault)),
                Start = operationConfig.ToString(nameof(RegionOperationData.Start)),
                End = operationConfig.ToString(nameof(RegionOperationData.End)),
                Include = operationConfig.ToBool(nameof(RegionOperationData.Include)),
                Trim = operationConfig.ToBool(nameof(RegionOperationData.Trim)),
                WholeLine = operationConfig.ToBool(nameof(RegionOperationData.WholeLine))
            };

            return regionOperationData;
        }

        private static ReplacementOperationData ReadReplacementOperationData(JObject operationConfig)
        {
            List<TokenLookaroundData> onlyIfData = new List<TokenLookaroundData>();

            JArray onlyIf = operationConfig.Get<JArray>("onlyIf");
            if (onlyIf != null)
            {
                foreach (JToken token in onlyIf.Children())
                {
                    if (!(token is JObject entryJObject))
                    {
                        continue;
                    }

                    TokenLookaroundData lookaround = new TokenLookaroundData()
                    {
                        Before = entryJObject.ToString(nameof(TokenLookaroundData.Before)),
                        After = entryJObject.ToString(nameof(TokenLookaroundData.After))
                    };
                    onlyIfData.Add(lookaround);
                }
            }

            ReplacementOperationData replacementOperationData = new ReplacementOperationData()
            {
                Id = operationConfig.ToString(nameof(ReplacementOperationData.Id)),
                OnByDefault = operationConfig.ToBool(nameof(ReplacementOperationData.OnByDefault)),
                Original = operationConfig.ToString(nameof(ReplacementOperationData.Original)),
                Replacement = operationConfig.ToString(nameof(ReplacementOperationData.Replacement)),
                OnlyIf = onlyIfData
            };

            return replacementOperationData;
        }

        private static UnsupportedTypeOperationData ReadUnsupportedTypeOperationData(JObject operationConfig)
        {
            UnsupportedTypeOperationData unsupportedData = new UnsupportedTypeOperationData()
            {
                Id = operationConfig.ToString(nameof(UnsupportedTypeOperationData.Id)),
                OnByDefault = operationConfig.ToBool(nameof(UnsupportedTypeOperationData.OnByDefault)),
                SpecifiedType = ConditionalCustomOperationData.OperationName,
                RawConfig = operationConfig.ToString()
            };

            return unsupportedData;
        }
    }
}
