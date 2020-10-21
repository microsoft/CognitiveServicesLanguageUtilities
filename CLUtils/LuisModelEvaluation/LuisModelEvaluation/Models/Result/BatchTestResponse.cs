// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using System.Collections.Generic;

namespace Microsoft.LuisModelEvaluation.Models.Result
{
    public class BatchTestResponse
    {
        /// <summary>
        /// Gets or sets a list of statistics about each classification model.
        /// </summary>
        public IReadOnlyList<ModelStats> ClassificationModelsStats { get; set; }

        /// <summary>
        /// Gets or sets a list of statistics about each entity model.
        /// </summary>
        public IReadOnlyList<ModelStats> EntityModelsStats { get; set; }

        /// <summary>
        /// Gets or sets a list of statistics about each query.
        /// </summary>
        public IReadOnlyList<QueryStats> QueryStats { get; set; }
    }
}