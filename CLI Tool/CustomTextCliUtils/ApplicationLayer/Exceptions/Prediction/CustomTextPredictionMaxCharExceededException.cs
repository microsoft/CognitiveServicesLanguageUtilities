using Microsoft.CustomTextCliUtils.Configs.Consts;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Prediction
{
    public class CustomTextPredictionMaxCharExceededException : CliException
    {
        public CustomTextPredictionMaxCharExceededException(int currCharCount)
            : base(ConstructMessage(currCharCount))
        { }

        public static string ConstructMessage(int currCharCount)
        {
            return $"Prediction failed. Your document is {currCharCount}-char long, but 'Custom Text' character limit is {Constants.CustomTextPredictionMaxCharLimit}.\nPlease consider using the chunking flag to chunk down your document before prediction!";
        }

    }
}
