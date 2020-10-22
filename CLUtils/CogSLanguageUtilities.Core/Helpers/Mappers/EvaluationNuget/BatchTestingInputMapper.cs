using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Enums.CustomText;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.AppModels.Response;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.LabeledExamples.Response;
using Microsoft.CogSLanguageUtilities.Definitions.Models.CustomText.Api.Prediction.Response.Result;
using Microsoft.LuisModelEvaluation.Models.Input;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.CogSLanguageUtilities.Core.Helpers.Mappers.EvaluationNuget
{
    /// <summary>
    /// The reason for creating an input mapper is that
    /// the evaluation nuget we're using takes in specific models for for evaluation
    /// thus, we need to map our application data to the format required by the nuget
    /// </summary>
    public class BatchTestingInputMapper
    {
        private const string instanceKey = "$instance";
        private const string typeKey = "type";
        private const string startIndexKey = "startIndex";
        private const string lengthKey = "length";

        /// <summary>
        /// Maps all entity models in the Custom Text app 
        /// </summary>
        /// <param name="customTextModels"></param>
        /// <param name="entityPrefix"></param>
        /// <returns>Entity models used by eval nuget</returns>
        public static List<Model> MapCustomTextAppEntityModels(List<CustomTextModel> customTextModels, string entityPrefix)
        {
            return customTextModels.SelectMany(m =>
            {
                List<Model> models = new List<Model>();
                if (m.TypeId != (int)ModelType.Cl)
                {
                    var modelName = string.IsNullOrEmpty(entityPrefix) ? m.Name : $"{entityPrefix}{Constants.ModelHierarchySeparator}{m.Name}";
                    models.Add(new Model
                    {
                        Name = modelName,
                        Type = m.ReadableType
                    });
                    if (m.Children?.Any() == true)
                    {
                        models.AddRange(MapCustomTextAppEntityModels(m.Children, modelName));
                    }
                }
                return models;
            }).ToList();
        }

        /// <summary>
        /// Maps all class models in the Custom Text app 
        /// </summary>
        /// <param name="customTextModels"></param>
        /// <returns>Class models used by eval nuget</returns>
        public static List<Model> MapCustomTextAppClassModels(List<CustomTextModel> customTextModels)
        {
            List<Model> models = new List<Model>();
            customTextModels.ForEach(m =>
            {
                if (m.TypeId == (int)ModelType.Cl)
                {
                    models.Add(new Model
                    {
                        Name = m.Name,
                        Type = m.ReadableType
                    });
                }
            });
            return models;
        }

        /// <summary>
        /// Maps Custom Text test example and prediction response
        /// </summary>
        /// <param name="documentText"></param>
        /// <param name="labeledExample"></param>
        /// <param name="predictionResponse"></param>
        /// <param name="modelsDictionary"></param>
        /// <returns>Testing Example model used by nuget</returns>
        public static TestingExample CreateTestExample(string documentText, Example labeledExample, CustomTextPredictionResponse predictionResponse, Dictionary<string, string> modelsDictionary)
        {
            var PredictionData = MapCutomTextPredictionResponse(predictionResponse);
            PredictionObject groundTruth = MapCustomTextLabeledExample(modelsDictionary, labeledExample);
            return new TestingExample
            {
                Text = documentText,
                LabeledData = groundTruth,
                PredictedData = PredictionData
            };
        }

        /// <summary>
        /// Maps prediction response model of CustomText
        /// </summary>
        /// <param name="customTextResponse"></param>
        /// <returns>PredictionObject used by evaluation nuget</returns>
        private static PredictionObject MapCutomTextPredictionResponse(CustomTextPredictionResponse customTextResponse)
        {
            return new PredictionObject
            {
                Classification = customTextResponse.Prediction.PositiveClassifiers ?? new List<string>(),
                Entities = MapCustomTextPredictionEntities(customTextResponse.Prediction.Extractors)
            };
        }

        /// <summary>
        /// Map CustomText labeled example
        /// </summary>
        /// <param name="modelsDictionary"></param>
        /// <param name="e"></param>
        /// <returns>PredictionObject used by evaluation nuget</returns>
        private static PredictionObject MapCustomTextLabeledExample(Dictionary<string, string> modelsDictionary, Example e)
        {
            var actualClassNames = e.ClassificationLabels?.Where(c => c.Label == true).Select(c => modelsDictionary[c.ModelId]).ToList();
            actualClassNames = actualClassNames ?? new List<string>();
            var groundTruth = new PredictionObject
            {
                Classification = actualClassNames,
                Entities = MapCustomTextLabeledEntities(e.MiniDocs, modelsDictionary)
            };
            return groundTruth;
        }

        private static List<Entity> MapCustomTextPredictionEntities(JObject extractors)
        {
            List<Entity> entities = new List<Entity>();
            var instance = extractors[instanceKey];
            foreach (var entry in extractors)
            {
                if (entry.Key != instanceKey)
                {
                    JArray entityArray = (JArray)entry.Value;
                    JArray instanceArray = (JArray)instance[entry.Key];
                    for (int i = 0; i < entityArray.Count; i++)
                    {
                        entities.Add(new Entity
                        {
                            Name = instanceArray[i][typeKey].ToString(),
                            StartPosition = instanceArray[i][startIndexKey].ToObject<int>(),
                            EndPosition = instanceArray[i][startIndexKey].ToObject<int>() + instanceArray[i][lengthKey].ToObject<int>(),
                            Children = entityArray[i] is JObject ? MapCustomTextPredictionEntities((JObject)entityArray[i]) : null
                        });
                    }
                }
            }
            return entities;
        }


        private static List<Entity> MapCustomTextLabeledEntities(List<MiniDoc> inputMiniDocs, Dictionary<string, string> modelsDictionary)
        {
            return inputMiniDocs.SelectMany(d => d.PositiveExtractionLabels).Select(e => MapCustomTextLabeledEntitiesInternal(e, modelsDictionary)).ToList();
        }

        private static Entity MapCustomTextLabeledEntitiesInternal(PositiveExtractionLabel extractionLabel, Dictionary<string, string> modelsDictionary)
        {
            if (extractionLabel == null)
            {
                return null;
            }
            return new Entity
            {
                Name = modelsDictionary[extractionLabel.ModelId],
                StartPosition = extractionLabel.StartCharIndex,
                EndPosition = extractionLabel.EndCharIndex,
                Children = extractionLabel.Children.Select(c => MapCustomTextLabeledEntitiesInternal(c, modelsDictionary)).ToList()
            };
        }
    }
}
