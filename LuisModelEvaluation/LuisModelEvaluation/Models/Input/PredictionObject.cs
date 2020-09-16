﻿using System.Collections.Generic;

namespace Microsoft.LuisModelEvaluation.Models.Input
{
    public class PredictionObject
    {
        public List<string> Classification { get; set; }
        public List<Entity> Entities { get; set; }
    }
}
