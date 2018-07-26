using System;
using System.Collections.Generic;
using Newtonsoft.Json.Linq;
using TemplateData.BaseLines;
using TemplateData.PostActions;
using TemplateData.PrimaryOutputs;
using TemplateData.Sources;
using TemplateReader.Utils;

namespace TemplateReader
{
    public static class TemplateReader
    {
        public static TemplateData.TemplateData ReadTemplateData(JObject source)
        {
            TemplateData.TemplateData configData = new TemplateData.TemplateData();

            ReadBasicConfigData(source, configData);

            configData.Forms = ValueFormReader.ReadValueForms(source);
            configData.Sources = ReadSources(source);
            configData.BaselineInfo = ReadBaselines(source);
            configData.Tags = source.ToStringDictionary(StringComparer.OrdinalIgnoreCase, nameof(TemplateData.TemplateData.Tags));
            configData.Symbols = SymbolReader.ReadSymbols(source);
            configData.PostActions = ReadPostActions(source);
            configData.PrimaryOutputs = ReadPrimaryOutputs(source);
            configData.CustomOperations = CustomFileGlobReader.ReadCustomFileGlobData(source.Get<JObject>("CustomOperations"), string.Empty);
            configData.GlobSpecificCustomOperations = CustomFileGlobReader.ReadGlobSpecificCustomOperations(source);

            return configData;
        }

        private static void ReadBasicConfigData(JObject source, TemplateData.TemplateData configData)
        {
            configData.Author = source.ToString(nameof(configData.Author));
            configData.Classifications = source.ArrayAsStrings(nameof(configData.Classifications));
            configData.DefaultName = source.ToString(nameof(configData.DefaultName));
            configData.Description = source.ToString(nameof(configData.Description));
            configData.GroupIdentity = source.ToString(nameof(configData.GroupIdentity));
            configData.Precedence = source.ToInt32(nameof(configData.Precedence));
            configData.Guids = source.ArrayAsGuids(nameof(configData.Guids));
            configData.Identity = source.ToString(nameof(configData.Identity));
            configData.Name = source.ToString(nameof(configData.Name));
            configData.SourceName = source.ToString(nameof(configData.SourceName));
            configData.PreferNameDirectory = source.ToBool(nameof(configData.PreferNameDirectory));
            configData.PlaceholderFilename = source.ToString(nameof(configData.PlaceholderFilename));
            configData.GeneratorVersions = source.ToString(nameof(configData.GeneratorVersions));

            JToken shortNameToken = source.Get<JToken>("ShortName");
            configData.ShortNameList = shortNameToken.JTokenStringOrArrayToCollection(new List<string>());
        }

        private static IReadOnlyList<FileSourceData> ReadSources(JObject source)
        {
            List<FileSourceData> sourceData = new List<FileSourceData>();

            foreach (JObject sourceSource in source.Items<JObject>(nameof(TemplateData.TemplateData.Sources)))
            {
                List<SourceModifierData> modifiersData = new List<SourceModifierData>();

                foreach (JObject modifierSource in sourceSource.Items<JObject>(nameof(FileSourceData.Modifiers)))
                {
                    modifiersData.Add(ReadSourceModifierData(modifierSource));
                }

                FileSourceData sourceConfig = new FileSourceData()
                {
                    Source = sourceSource.ToString(nameof(sourceConfig.Source)),
                    Target = sourceSource.ToString(nameof(sourceConfig.Target)),
                    TopLevelFileOperations = ReadSourceModifierData(sourceSource),
                    Modifiers = modifiersData
                };

                sourceData.Add(sourceConfig);
            }

            return sourceData;
        }

        private static SourceModifierData ReadSourceModifierData(JObject source)
        {
            SourceModifierData sourceModifierData = new SourceModifierData()
            {
                Condition = source.ToString(nameof(SourceModifierData.Condition)),
                CopyOnly = source.Get<JToken>(nameof(SourceModifierData.CopyOnly))
                                        .JTokenStringOrArrayToCollection(null),
                Include = source.Get<JToken>(nameof(SourceModifierData.Include))
                                        .JTokenStringOrArrayToCollection(null),
                Exclude = source.Get<JToken>(nameof(SourceModifierData.Exclude))
                                        .JTokenStringOrArrayToCollection(null),
                Rename = source.Get<JToken>(nameof(SourceModifierData.Rename))
                                        .ToStringDictionary()
            };

            return sourceModifierData;
        }

        private static IReadOnlyDictionary<string, BaseLineData> ReadBaselines(JObject source)
        {
            Dictionary<string, BaseLineData> baselineData = new Dictionary<string, BaseLineData>();

            foreach (JProperty property in source.PropertiesOf("baselines"))
            {
                JObject propertyAsJObject = property.Value as JObject;

                if (propertyAsJObject == null)
                {
                    continue;
                }

                BaseLineData baseline = new BaseLineData()
                {
                    Description = propertyAsJObject.ToString(nameof(BaseLineData.Description)),
                    DefaultOverrides = propertyAsJObject.Get<JObject>(nameof(BaseLineData.DefaultOverrides)).ToStringDictionary()
                };

                baselineData[property.Name] = baseline;
            }

            return baselineData;
        }

        public static IReadOnlyList<PostActionData> ReadPostActions(JObject source)
        {
            List<PostActionData> postActionList = new List<PostActionData>();

            foreach (JToken action in source.Get<JArray>(nameof(TemplateData.TemplateData.PostActions)))
            {
                Dictionary<string, string> args = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
                foreach (JProperty argInfo in action.PropertiesOf(nameof(PostActionData.Args)))
                {
                    args.Add(argInfo.Name, argInfo.Value.ToString());
                }

                List<PostActionInstruction> instructionList = new List<PostActionInstruction>();
                JArray manualInstructions = action.Get<JArray>(nameof(PostActionData.ManualInstructions));

                foreach (JToken textAndCondition in manualInstructions)
                {
                    PostActionInstruction instruction = new PostActionInstruction()
                    {
                        Text = textAndCondition.ToString(nameof(PostActionInstruction.Text)),
                        Condition = textAndCondition.ToString(nameof(PostActionInstruction.Condition))
                    };
                    instructionList.Add(instruction);
                }

                PostActionData postAction = new PostActionData()
                {
                    Condition = action.ToString(nameof(PostActionData.Condition)),
                    Description = action.ToString(nameof(PostActionData.Description)),
                    ActionId = action.ToGuid(nameof(PostActionData.ActionId)),
                    ContinueOnError = action.ToBool(nameof(PostActionData.ContinueOnError)),
                    Args = args,
                    ManualInstructions = instructionList,
                    ConfigFile = action.ToString(nameof(PostActionData.ConfigFile))
                };

                postActionList.Add(postAction);
            }

            return postActionList;
        }

        public static IReadOnlyList<PrimaryOutputData> ReadPrimaryOutputs(JObject source)
        {
            List<PrimaryOutputData> primaryOutputList = new List<PrimaryOutputData>();

            JArray primaryOutputSource = source.Get<JArray>(nameof(TemplateData.PrimaryOutputs));

            if (primaryOutputSource == null)
            {
                return primaryOutputList;
            }

            foreach (JToken outputJToken in primaryOutputSource)
            {
                PrimaryOutputData output  = new PrimaryOutputData()
                {
                    Path = outputJToken.ToString(nameof(PrimaryOutputData.Path)),
                    Condition = outputJToken.ToString(nameof(PrimaryOutputData.Condition))
                };
                primaryOutputList.Add(output);
            }

            return primaryOutputList;
        }
    }
}
