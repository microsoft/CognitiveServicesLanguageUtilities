using FileFormatConverter.Core.Interfaces;
using System;
using System.IO;

namespace FileFormatConverter.Services
{
    public class LocalFileHandlerService : IFileHandler
    {
        public string ReadFile(string filePath)
        {
            if (File.Exists(filePath))
            {
                return File.ReadAllText(filePath);
            }
            throw new Exception("File not found!");
        }

        public bool WriteFile(string filePath, string content)
        {
            File.WriteAllText(filePath, content);
            return true;
        }
    }
}