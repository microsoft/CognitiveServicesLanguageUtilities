// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿
namespace Microsoft.LuisModelEvaluation.Models.Result
{
    public class EntityNameAndLocation
    {
        public string EntityName { get; set; }

        public int StartPosition { get; set; }

        public int EndPosition { get; set; }
    }
}