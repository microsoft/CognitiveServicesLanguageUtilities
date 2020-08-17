using System;
using System.Collections.Generic;
using System.Text;

namespace CliTool.Configs
{
    class ConfigKeys
    {
        //MS Read
        public readonly string CognitiveServicesKey = "cognitive-services-key";
        public readonly string CognitiveServicesEndpoint = "cognitive-services-endpoint";
        //Tika
        public readonly string EnableOcr = "enable-ocr";
        public readonly string DetectTiltedText = "detect-tilted-text";
        public readonly string SortByPostition = "sort-by-position";
        //Storage
        public readonly string SourceDir = "source-dir";
        public readonly string DestinationDir = "destination-dir";
        public readonly string SourceContainer = "source-container";
        public readonly string DestinationContainer = "destination-container";
        public readonly string Connectionstring = "connection-string";
    }
}
