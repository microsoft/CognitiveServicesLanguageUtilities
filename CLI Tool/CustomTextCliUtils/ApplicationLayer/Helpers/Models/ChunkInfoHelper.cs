using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CustomTextCliUtils.ApplicationLayer.Helpers.Models
{
    public class ChunkInfoHelper
    {
        public static string GetChunkFileName(string fileName, int chunkNumber)
        {
            return $"{Path.GetFileNameWithoutExtension(fileName)}_{chunkNumber + 1}.txt";
        }
    }
}
