// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;

namespace Microsoft.CogSLanguageUtilities.ViewLayer.CliCommands.Configs.Consts
{
    public class CommandOptionTemplate
    {
        // msread
        public const string MSReadAzureResourceKey = "--" + ConfigKeys.MSReadAzureResourceKey + " <COGNITIVE_SERVICES_KEY>";
        public const string MSReadAzureResourceEndpoint = "--" + ConfigKeys.MSReadAzureResourceEndpoint + " <ENDPOINT_URL>";

        // local storage
        public const string LocalStorageSourceDir = "--" + ConfigKeys.LocalStorageSourceDir + " <ABSOLUTE_PATH>";
        public const string LocalStorageDestinationDir = "--" + ConfigKeys.LocalStorageDestinationDir + " <ABSOLUTE_PATH>";

        // blob storage
        public const string BlobStorageSourceContainer = "--" + ConfigKeys.BlobStorageSourceContainer + " <CONTAINER_NAME>";
        public const string BlobStorageDestinationContainer = "--" + ConfigKeys.BlobStorageDestinationContainer + " <CONTAINER_NAME>";
        public const string BlobStorageConnectionstring = "--" + ConfigKeys.BlobStorageConnectionstring + " <CONNECTION_STRING>";

        // chunker
        public const string ChunkerCharLimit = "--" + ConfigKeys.ChunkerCharLimit + " <INTEGER>";
        public const string ChunkerSectionLevel = "--" + ConfigKeys.ChunkerSectionLevel + " <TITLE/H1/H2/H3>";

        // prediction
        public const string CustomTextAzureResourceKey = "--" + ConfigKeys.CustomTextAzureResourceKey + " <CUSTOM_TEXT_KEY>";
        public const string CustomTextAzureResourceEndpoint = "--" + ConfigKeys.CustomTextAzureResourceEndpoint + " <ENDPOINT_URL>";
        public const string CustomTextAppId = "--" + ConfigKeys.CustomTextAppId + " <APP_ID>";

        // text analytics
        public const string TextAnalyticsAzureResourceKey = "--" + ConfigKeys.TextAnalyticsAzureResourceKey + " <TEXT_ANALYTICS_KEY>";
        public const string TextAnalyticsAzureResourceEndpoint = "--" + ConfigKeys.TextAnalyticsAzureResourceEndpoint + " <TEXT_ANALYTICS_URL>";
        public const string TextAnalyticsDefaultLanguage = "--" + ConfigKeys.TextAnalyticsDefaultLanguage + " <DEFAULT_LANGUAGE>";

        public const string TextAnalyticsEnableSentiment = "--" + ConfigKeys.TextAnalyticsSentiment + " <BOOL>";
        public const string TextAnalyticsEnableNer = "--" + ConfigKeys.TextAnalyticsNer + " <BOOL>";
        public const string TextAnalyticsEnableKeyphrase = "--" + ConfigKeys.TextAnalyticsKeyphrase + " <BOOL>";

        // evaluation
        public const string EvaluationLabeledExamplesAppAzureResourceKey = "--" + ConfigKeys.EvaluationLabeledExamplesAppAzureResourceKey + " <RESOURCE_KEY>";
        public const string EvaluationLabeledExamplesAppAzureResourceEndpoint = "--" + ConfigKeys.EvaluationLabeledExamplesAppAzureResourceEndpoint + " <ENDPOINT_URL>";
        public const string EvaluationLabeledExamplesAppId = "--" + ConfigKeys.EvaluationLabeledExamplesAppId + " <APP_ID>";

    }
}