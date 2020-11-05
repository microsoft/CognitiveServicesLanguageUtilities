// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.APIs.Configs;
using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Exceptions.Configs;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.CustomText;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Parser;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.Storage;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Configs.TextAnalytics;
using Newtonsoft.Json;
using System.IO;

namespace Microsoft.CustomTextCliUtils.Configs
{
    public class ConfigsLoader : IConfigsLoader
    {
        readonly ConfigModel _configModel;

        public ConfigsLoader()
        {
            var filePath = Path.Combine(Constants.ConfigsFileLocalDirectory, Constants.ConfigsFileName);
            if (File.Exists(filePath))
            {
                var configsFile = File.ReadAllText(filePath);
                _configModel = JsonConvert.DeserializeObject<ConfigModel>(configsFile);
                SetDefaults();
            }
            else
            {
                throw new MissingConfigsException();
            }
        }

        public BlobStorageConfigModel GetBlobConfigModel()
        {
            return _configModel.Storage.Blob;
        }

        public ChunkerConfigModel GetChunkerConfigModel()
        {
            return _configModel.Chunker;
        }

        public LocalStorageConfigModel GetLocalConfigModel()
        {
            return _configModel.Storage.Local;
        }

        public MSReadConfigModel GetMSReadConfigModel()
        {
            return _configModel.Parser.MsRead;
        }

        public CustomTextAppConfigModel GetCustomTextPredictionConfigModel()
        {
            return _configModel.CustomText.Prediction;
        }

        public CustomTextAppConfigModel GetCustomTextAuthoringConfigModel()
        {
            return _configModel.CustomText.Authoring;
        }

        public StorageConfigModel GetStorageConfigModel()
        {
            return _configModel.Storage;
        }

        public TextAnalyticsConfigModel GetTextAnalyticsConfigModel()
        {
            return _configModel.TextAnalytics;
        }

        public ParserConfigModel GetParserConfigModel()
        {
            return _configModel.Parser;
        }
        
        private void SetDefaults()
        {
            if (_configModel.Chunker.CharLimit == 0)
            {
                _configModel.Chunker.CharLimit = Constants.DefaultCharLimit;
            }
        }
    }
}
