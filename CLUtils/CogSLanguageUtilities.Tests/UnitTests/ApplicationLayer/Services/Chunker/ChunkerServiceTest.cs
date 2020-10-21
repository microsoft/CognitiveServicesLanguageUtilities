// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
﻿using Microsoft.CogSLanguageUtilities.Core.Services.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.APIs.Services;
using Microsoft.CogSLanguageUtilities.Definitions.Configs.Consts;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Enums.Chunker;
using Microsoft.CogSLanguageUtilities.Definitions.Models.Parser;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace Microsoft.CogSLanguageUtilities.Tests.UnitTests.Services.Chunker
{
    public class ChunkerServiceTest
    {
        public static TheoryData NoChunkingTestData()
        {
            string inputString1 = File.ReadAllText(@"TestData\Chunker\loremipsum-4.json");
            ParsedDocument testInput1 = JsonConvert.DeserializeObject<ParsedDocument>(inputString1);
            string expectedString1 = File.ReadAllText(@"TestData\Chunker\NoChunking\loremipsum-4_chunks.json");
            IEnumerable<ChunkInfo> output1 = JsonConvert.DeserializeObject<IEnumerable<ChunkInfo>>(expectedString1);
            return new TheoryData<ParsedDocument, IEnumerable<ChunkInfo>>
            {
                {
                    testInput1,
                    output1
                }
            };
        }

        public static TheoryData PageChunkingTestData()
        {
            string inputString1 = File.ReadAllText(@"TestData\Chunker\loremipsum-4.json");
            ParsedDocument testInput1 = JsonConvert.DeserializeObject<ParsedDocument>(inputString1);
            string expectedString1 = File.ReadAllText(@"TestData\Chunker\PageChunking\loremipsum-4_chunks.json");
            IEnumerable<ChunkInfo> output1 = JsonConvert.DeserializeObject<IEnumerable<ChunkInfo>>(expectedString1);
            return new TheoryData<ParsedDocument, IEnumerable<ChunkInfo>>
            {
                {
                    testInput1,
                    output1
                }
            };
        }

        public static TheoryData CharChunkingTestData()
        {
            string inputString1 = File.ReadAllText(@"TestData\Chunker\loremipsum-4.json");
            ParsedDocument testInput1 = JsonConvert.DeserializeObject<ParsedDocument>(inputString1);
            string expectedString1 = File.ReadAllText(@"TestData\Chunker\CharChunking\loremipsum-4_chunks.json");
            IEnumerable<ChunkInfo> output1 = JsonConvert.DeserializeObject<IEnumerable<ChunkInfo>>(expectedString1);
            return new TheoryData<ParsedDocument, IEnumerable<ChunkInfo>>
            {
                {
                    testInput1,
                    output1
                }
            };
        }

        [Theory]
        [MemberData(nameof(NoChunkingTestData))]
        public void NoChunkingTest(ParsedDocument parseResult, List<ChunkInfo> expectedChunks)
        {
            IChunkerService msReadChunker = new ChunkerService();
            List<ChunkInfo> actualChunks = msReadChunker.Chunk(parseResult, ChunkMethod.NoChunking, 0);
            Assert.Equal(expectedChunks.Count, actualChunks.Count);
            Assert.Equal(expectedChunks, actualChunks, new ChunkInfoComparer());
        }

        [Theory]
        [MemberData(nameof(PageChunkingTestData))]
        public void PageChunkingTest(ParsedDocument parseResult, List<ChunkInfo> expectedChunks)
        {
            IChunkerService msReadChunker = new ChunkerService();
            List<ChunkInfo> actualChunks = msReadChunker.Chunk(parseResult, ChunkMethod.Page, Constants.CustomTextPredictionMaxCharLimit);
            Assert.Equal(expectedChunks.Count, actualChunks.Count);
            Assert.Equal(expectedChunks, actualChunks, new ChunkInfoComparer());
        }

        [Theory]
        [MemberData(nameof(CharChunkingTestData))]
        public void CharChunkingTest(ParsedDocument parseResult, List<ChunkInfo> expectedChunks)
        {
            IChunkerService msReadChunker = new ChunkerService();
            List<ChunkInfo> actualChunks = msReadChunker.Chunk(parseResult, ChunkMethod.Char, 1000);
            Assert.Equal(expectedChunks.Count, actualChunks.Count);
            Assert.Equal(expectedChunks, actualChunks, new ChunkInfoComparer());
        }
    }

    public class ChunkInfoComparer : IEqualityComparer<ChunkInfo>
    {
        public bool Equals(ChunkInfo x, ChunkInfo y)
        {
            return x.Summary == y.Summary && x.StartPage == y.StartPage && x.EndPage == y.EndPage;
        }

        public int GetHashCode(ChunkInfo obj)
        {
            return obj.GetHashCode();
        }
    }
}
