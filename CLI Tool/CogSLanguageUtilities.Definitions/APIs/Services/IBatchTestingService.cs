using Microsoft.LuisModelEvaluation.Models.Input;
using System.Collections.Generic;

namespace Microsoft.CogSLanguageUtilities.Definitions.APIs.Services
{
    public interface IBatchTestingService
    {
        public LuisModelEvaluation.Models.Result.BatchTestResponse RunBatchTest(List<TestingExample> testData, List<Model> entities, List<Model> classes);
    }
}
