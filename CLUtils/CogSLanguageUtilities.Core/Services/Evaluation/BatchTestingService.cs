using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.LuisModelEvaluation.Controllers;
using Microsoft.LuisModelEvaluation.Models.Input;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Core.Services.Evaluation
{
    public class BatchTestingService : IBatchTestingService
    {
        public LuisModelEvaluation.Models.Result.BatchTestResponse RunBatchTest(List<TestingExample> testData, List<Model> entities, List<Model> classes)
        {
            var evaluation = new EvaluationController();
            return evaluation.EvaluateModel(testData, verbose: true, entities, classes);
        }
    }
}
