// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Definitions.Helpers;
using Newtonsoft.Json;

namespace Microsoft.CogSLanguageUtilities.Definitions.Models.Chunker
{
    public class ChunkInfo
    {
        public int ChunkNumber { get; set; }
        public int CharCount { get; set; }
        public int? StartPage { get; set; }
        public int? EndPage { get; set; }
        public string Summary { get; set; }
        [JsonIgnore]
        public string Text { get; set; }
        public ChunkInfo(int chunkNumber, string chunkText, int? startPage, int? endPage)
        {
            ChunkNumber = chunkNumber;
            var text = chunkText.Trim();
            Text = text;
            CharCount = text.Length;
            StartPage = startPage;
            EndPage = endPage;
            Summary = ChunkInfoHelper.GetChunksummary(text);
        }
        public ChunkInfo(string chunkText)
        {
            var text = chunkText.Trim();
            Text = text;
            Summary = ChunkInfoHelper.GetChunksummary(text);
        }
        public ChunkInfo()
        { }
    }
}
