namespace FileFormatConverter.Orchestrators
{
    internal class FileWriterService
    {
        public static void WriteToFile(string filePath, string contents)
        {
            File.WriteAllText(filePath, contents);
        }
    }
}