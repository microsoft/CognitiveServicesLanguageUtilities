
using FileFormatConverter.Orchestrators;

namespace FileFormatConverter
{
    public class Start
    {
        public static void Main(string[] args)
        {
            TestJsonLToCustomText();
        }

        public static void TestJsonLToCustomText()
        {
            var inputFilePath = @"C:\Users\mshaban\Desktop\cli tool\file samples\source-labeled_datapoints.jsonl";
            var targetFilePath = @"C:\Users\mshaban\Desktop\cli tool\file samples\test.json";
            FileConversionOrchestrator.ConvertModelFile(inputFilePath, targetFilePath);
        }
    }
}
