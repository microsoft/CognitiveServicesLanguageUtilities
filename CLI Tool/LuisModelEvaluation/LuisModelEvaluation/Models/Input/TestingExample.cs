namespace Microsoft.LuisModelEvaluation.Models.Input
{
    public class TestingExample
    {
        public string Text { get; set; }
        public PredictionObject LabeledData { get; set; }
        public PredictionObject PredictedData { get; set; }
    }
}
