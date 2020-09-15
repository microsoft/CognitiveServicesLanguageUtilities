namespace Microsoft.LuisModelEvaluation.Models.Evaluation
{
    /*
     * MUC stands for "Message Understanding Conference"
     * https://en.wikipedia.org/wiki/Message_Understanding_Conference
     * Entity evaluation specifics can be found in the following paper
     * https://nlp.cs.nyu.edu/sekine/papers/li07.pdf
     */
    public class MucEntityConfusionMatrix

    {
        /// <summary>
        /// Gets or sets the model name
        /// </summary>
        public string ModelName { get; set; }

        /// <summary>
        /// Gets or sets the model type
        /// </summary>
        public string ModelType { get; set; }

        /// <summary>
        /// Gets or sets the count of entities that have correct type
        /// </summary>
        public int CorrectTypeCount { get; set; }

        /// <summary>
        /// Gets or sets the count of entities that have correct text location
        /// </summary>
        public int CorrectTextCount { get; set; }

        /// <summary>
        /// Gets or sets the count of actual entities present in the input
        /// </summary>
        public int ActualCount { get; set; }

        /// <summary>
        /// Gets or sets the count of predicted entities
        /// </summary>
        public int PossibleCount { get; set; }

        public double CalculatePrecision()
        {
            return ((double)CorrectTextCount + CorrectTypeCount) / (2 * ActualCount);
        }

        public double CalculateRecall()
        {
            return ((double)CorrectTextCount + CorrectTypeCount) / (2 * PossibleCount);
        }

        public double CalculateFScore()
        {
            double precision = CalculatePrecision();
            double recall = CalculateRecall();
            return (2 * precision * recall) / (precision + recall);
        }

        public double CalculateTextPrecision()
        {
            return ((double)CorrectTextCount) / ActualCount;
        }

        public double CalculateTextRecall()
        {
            return ((double)CorrectTextCount) / PossibleCount;
        }

        public double CalculateTextFScore()
        {
            double precision = CalculateTextPrecision();
            double recall = CalculateTextRecall();
            return (2 * precision * recall) / (precision + recall);
        }

        public double CalculateTypePrecision()
        {
            return ((double)CorrectTypeCount) / ActualCount;
        }

        public double CalculateTypeRecall()
        {
            return ((double)CorrectTypeCount) / PossibleCount;
        }

        public double CalculateTypeFScore()
        {
            double precision = CalculateTypePrecision();
            double recall = CalculateTypeRecall();
            return (2 * precision * recall) / (precision + recall);
        }
    }
}
