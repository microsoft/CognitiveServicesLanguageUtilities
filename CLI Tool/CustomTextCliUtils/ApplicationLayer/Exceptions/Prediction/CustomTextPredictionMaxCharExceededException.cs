using Microsoft.CustomTextCliUtils.Configs.Consts;

namespace Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Prediction
{
    class CustomTextPredictionMaxCharExceededException : CliException
    {
        public CustomTextPredictionMaxCharExceededException(int currCharCount)
            : base(ConstructMessage(currCharCount))
        { }

        public static string ConstructMessage(int currCharCount)
        {
            return $"Sorry your document is {currCharCount}-char long, but 'Custom Text' prediction endpoint only supports text documents up to {Constants.CustomTextPredictionMaxCharLimit}.\nPlease consider using the chunking flag to chunk down your document before prediction!";
        }

    }
}
