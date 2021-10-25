// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿namespace Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts
{
    public class ConfigKeys
    {
        // msread
        public const string MSReadAzureResourceKey = "azure-resource-key";
        public const string MSReadAzureResourceEndpoint = "azure-resource-endpoint";

        // local storage
        public const string LocalStorageSourceDir = "source-dir";
        public const string LocalStorageDestinationDir = "destination-dir";

        // blob storage
        public const string BlobStorageSourceContainer = "source-container";
        public const string BlobStorageDestinationContainer = "destination-container";
        public const string BlobStorageConnectionstring = "connection-string";

        // chunker
        public const string ChunkerCharLimit = "char-limit";
        public const string ChunkerSectionLevel = "chunk-section-level";

        // prediction
        public const string CustomTextAzureResourceKey = "azure-resource-key";
        public const string CustomTextAzureResourceEndpoint = "azure-resource-endpoint";
        public const string CustomTextAppId = "app-id";

        // text analytics
        public const string TextAnalyticsAzureResourceKey = "azure-resource-key";
        public const string TextAnalyticsAzureResourceEndpoint = "azure-resource-endpoint";
        public const string TextAnalyticsDefaultLanguage = "default-language";
        public const string TextAnalyticsSentiment = "sentiment";
        public const string TextAnalyticsNer = "ner";
        public const string TextAnalyticsKeyphrase = "keyphrase";

        // evaluation
        public const string EvaluationLabeledExamplesAppAzureResourceKey = "azure-resource-key";
        public const string EvaluationLabeledExamplesAppAzureResourceEndpoint = "azure-resource-endpoint";
        public const string EvaluationLabeledExamplesAppId = "app-id";

    }
}
