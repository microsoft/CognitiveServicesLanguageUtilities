// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;

namespace Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Prediction
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
