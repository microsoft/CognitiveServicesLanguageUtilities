using System;
using System.Collections.Generic;
using System.Text;

namespace CustomTextCliUtils.ApplicationLayer.Modeling.Exceptions
{
    public enum CliExceptionCode
    {
        // Config Exceptions
        MissingConfig = 100,
        // Storage Exceptions
        BlobContainerNotFound = 200,
        FolderNotFound = 201,
        InvalidBlobStorageConnectionString = 203,
        Unauthorized = 204,
        // Parser Exceptions
        MsReadConnection = 300,
        UnsupportedFileType = 301,
        // Prediction
        CustomTextPredictionMaxCharExceeded = 400
    }
}
