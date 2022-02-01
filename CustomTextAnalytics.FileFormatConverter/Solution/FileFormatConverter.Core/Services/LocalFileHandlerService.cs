using FileFormatConverter.Core.Interfaces;
using System;
using System.IO;

namespace FileFormatConverter.Services
{
    public class LocalFileHandlerService : IFileHandler
    {
        public string ReadFileAsString(string filePath)
        {
            try
            {
                var content = File.ReadAllText(filePath);
                return content;
            }
            catch (FileNotFoundException)
            {
                throw new Exception("File not found!");
            }
            catch (Exception)
            {
                throw new Exception("Couldn't read file content!");
            }
        }

        public bool WriteFileAsString(string filePath, string content)
        {
            try
            {
                File.WriteAllText(filePath, content);
                return true;
            }
            catch (Exception)
            {
                throw new Exception("Couldn't write to file!");
            }
        }
    }
}