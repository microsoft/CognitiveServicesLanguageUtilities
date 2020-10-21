// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.Models.Evaluation;
using Microsoft.LuisModelEvaluation.Models.Result;
using System.Linq;

namespace Microsoft.CogSLanguageUtilities.Core.Helpers.Mappers.EvaluationNuget
{
    /// <summary>
    /// The reason for creating an output mapper is that
    /// we want to display the batch testing response to the user in a specifc format
    /// hence, we need to map the response returned by the evaluation nuget to our desired format
    /// </summary>
    public class BatchTestingOutputMapper
    {
        /// <summary>
        /// Map batch testing result returned from nuget
        /// </summary>
        /// <param name="batchTestResponse"></param>
        /// <returns>Modified evaluation model</returns>
        public static BatchTestingOutput MapEvaluationOutput(BatchTestResponse batchTestResponse)
        {
            return new BatchTestingOutput
            {
                ClassificationModelsStats = batchTestResponse.ClassificationModelsStats.Select(m => MapModelStats(m)).ToList(),
                EntityModelsStats = batchTestResponse.EntityModelsStats.Select(m => MapModelStats(m)).ToList(),
                DocumentsStats = batchTestResponse.QueryStats.Select(m => MapQueryStats(m)).ToList()
            };
        }

        private static BatchTestingModelStats MapModelStats(ModelStats modelStats)
        {
            return new BatchTestingModelStats
            {
                EntityTextFScore = modelStats.EntityTextFScore,
                EntityTypeFScore = modelStats.EntityTypeFScore,
                FScore = modelStats.FScore,
                ModelName = modelStats.ModelName,
                ModelType = modelStats.ModelType,
                Precision = modelStats.Precision,
                Recall = modelStats.Recall
            };
        }

        private static BatchTestingDocumentStats MapQueryStats(QueryStats queryStats)
        {
            return new BatchTestingDocumentStats
            {
                FalseNegativeEntities = queryStats.FalseNegativeEntities.Select(e => MapEntityNameAndLocation(e)).ToList(),
                FalsePositiveEntities = queryStats.FalsePositiveEntities.Select(e => MapEntityNameAndLocation(e)).ToList(),
                LabeledClassNames = queryStats.LabeledClassNames,
                PredictedClasstNames = queryStats.PredictedClassNames,
                DocumentText = queryStats.QueryText
            };
        }

        private static BatchTestingEntityDetails MapEntityNameAndLocation(EntityNameAndLocation entity)
        {
            return new BatchTestingEntityDetails
            {
                EndPosition = entity.EndPosition,
                EntityName = entity.EntityName,
                StartPosition = entity.StartPosition
            };
        }
    }
}