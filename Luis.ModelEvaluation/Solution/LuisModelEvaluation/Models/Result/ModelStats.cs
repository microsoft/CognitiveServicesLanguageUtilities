// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿
namespace Microsoft.LuisModelEvaluation.Models.Result
{
    public class ModelStats
    {
        public string ModelName { get; set; }

        public string ModelType { get; set; }

        public double Precision { get; set; }

        public double Recall { get; set; }

        public double FScore { get; set; }

        public double? EntityTextFScore { get; set; }

        public double? EntityTypeFScore { get; set; }
    }
}