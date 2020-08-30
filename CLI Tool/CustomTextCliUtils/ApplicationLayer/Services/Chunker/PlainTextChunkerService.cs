using Microsoft.CustomTextCliUtils.ApplicationLayer.Exceptions.Parser;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Chunker;
using Microsoft.CustomTextCliUtils.ApplicationLayer.Modeling.Models.Parser;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace  Microsoft.CustomTextCliUtils.ApplicationLayer.Services.Chunker
{
    class PlainTextChunkerService : IChunkerService
    {
        public List<ChunkInfo> Chunk(ParseResult parseResult, ChunkMethod chunkMethod, int charLimit)
        {
            throw new NotImplementedException();
        }

        public List<ChunkInfo> Chunk(string text, int charLimit)
        {
            var sentences = text.Split('.');
            var currentChunk = new StringBuilder();
            var resultChunks = new List<ChunkInfo>();
            foreach (var sentence in sentences.Select((value, i) => new { i, value }))
            {
                if (currentChunk.Length + sentence.value.Length < charLimit)
                {
                    currentChunk.Append(sentence);
                }
                else if (currentChunk.Length > 0)
                {
                    var chunkInfo = new ChunkInfo(currentChunk.ToString());
                    resultChunks.Add(chunkInfo);
                    currentChunk.Clear();
                }
                if (sentence.value.Length > charLimit)
                {
                    resultChunks.AddRange(ChunkBySpace(sentence.value, charLimit));
                }
                if (sentence.i == sentences.Length - 1 && currentChunk.Length > 0)
                {
                    var chunkInfo = new ChunkInfo(currentChunk.ToString());
                    resultChunks.Add(chunkInfo);
                }
            }
            return resultChunks;
        }

        public void ValidateFileType(string fileName)
        {
            var txtExtension = ".txt";
            string fileExtension = Path.GetExtension(fileName);
            if (fileExtension != txtExtension)
            {
                throw new UnsupportedFileTypeException(fileName, fileExtension, new string[] { txtExtension });
            }
        }

        private List<ChunkInfo> ChunkBySpace(string sentence, int charLimit)
        {
            var words = sentence.Split(' ');
            var currentSentence = new StringBuilder();
            var resultSentences = new List<ChunkInfo>();
            foreach (var word in words.Select((value, i) => new { i, value }))
            {
                if (currentSentence.Length + word.value.Length < charLimit)
                {
                    currentSentence.Append(word.value);
                }
                else
                {
                    var chunkInfo = new ChunkInfo(currentSentence.ToString());
                    resultSentences.Add(chunkInfo);
                    currentSentence.Clear();
                    currentSentence.Append(word.value);
                }
                if (word.i == words.Length - 1 && currentSentence.Length > 0)
                {
                    var chunkInfo = new ChunkInfo(currentSentence.ToString());
                    resultSentences.Add(chunkInfo);
                }
            }
            return resultSentences;
        }
    }
}
