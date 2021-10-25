// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.Azure.CognitiveServices.Language.LUIS.Runtime.Models;
using Microsoft.IAPUtilities.Definitions.Models.IAP;
using Microsoft.IAPUtilities.Definitions.Models.Luis;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.IAPUtilities.Core.Helpers.Mappers.Luis
{
    public class LuisOutputMapper
    {
        private const string InstanceKey = "$instance";
        private const string TypeKey = "type";
        private const string StartIndexKey = "startIndex";
        private const string TextKey = "text";

        public static CustomLuisResponse MapToCustomLuisRespone(PredictionResponse luisResponse)
        {
            return new CustomLuisResponse
            {
                Query = luisResponse.Query,
                TopIntent = luisResponse.Prediction.TopIntent,
                Intents = MapLuisPredictionIntents(luisResponse.Prediction.Intents),
                Entities = MapLuisPredictionEntities(JObject.FromObject(luisResponse.Prediction.Entities))
            };
        }

        private static Dictionary<string, double?> MapLuisPredictionIntents(IDictionary<string, Intent> intents)
        {
            return intents.ToDictionary(intent => intent.Key, intent => intent.Value.Score);
        }

        private static List<Extraction> MapLuisPredictionEntities(JObject extractors)
        {
            List<Extraction> entities = new List<Extraction>();
            var instance = extractors[InstanceKey];
            foreach (var entry in extractors)
            {
                try
                {
                    if (entry.Key != InstanceKey)
                    {
                        JArray entityArray = (JArray)entry.Value;
                        JArray instanceArray = (JArray)instance[entry.Key];
                        for (int i = 0; i < entityArray.Count; i++)
                        {
                            entities.Add(new Extraction
                            {
                                Text = instanceArray[i][TextKey].ToString(),
                                EntityName = instanceArray[i][TypeKey].ToString(),
                                Position = instanceArray[i][StartIndexKey].ToObject<int>(),
                                Children = entityArray[i] is JObject ? MapLuisPredictionEntities((JObject)entityArray[i]) : null
                            });
                        }
                    }
                }
                catch (Exception)
                {
                    // TODO: Handle all types of entities instead of skipping
                    continue;
                }
            }
            return entities.Count > 0 ? entities : null;
        }
    }
}
