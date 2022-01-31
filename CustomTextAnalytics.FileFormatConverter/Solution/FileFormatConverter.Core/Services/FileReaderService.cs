using System;
using System.IO;

namespace FileFormatConverter.Orchestrators
{
    public class FileReaderService
    {
        public static string Load(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            throw new Exception("File not found!");
        }
    }
}