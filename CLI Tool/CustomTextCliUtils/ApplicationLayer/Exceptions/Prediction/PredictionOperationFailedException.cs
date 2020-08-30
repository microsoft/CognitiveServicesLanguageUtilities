namespace  Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Prediction
{
    class PredictionOperationFailedException : CliException
    {
        public PredictionOperationFailedException(string operationId)
            : base(ConstructMessage(operationId))
        { }

        public static string ConstructMessage(string operationId)
        {
            return $"Prediction operation with ID {operationId} failed";
        }
    }
}
