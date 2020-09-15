namespace Microsoft.LuisModelEvaluation.Models.Evaluation
{
    public class ConfusionMatrix
    {
        public string ModelName { get; set; }

        public string ModelType { get; set; }

        public int TruePositives { get; set; }

        public int TrueNegatives { get; set; }

        public int FalsePositives { get; set; }

        public int FalseNegatives { get; set; }

        public double CalculatePrecision()
        {
            return (double)TruePositives / (TruePositives + FalsePositives);
        }

        public double CalculateRecall()
        {
            return (double)TruePositives / (TruePositives + FalseNegatives);
        }

        public double CalculateFScore()
        {
            double precision = CalculatePrecision();
            double recall = CalculateRecall();
            return 2 * precision * recall / (precision + recall);
        }
    }
}