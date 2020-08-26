using CustomTextCliUtils.ApplicationLayer.Modeling.Enums.Misc;
using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Chunker;
using CustomTextCliUtils.ApplicationLayer.Modeling.Models.Parser;
using CustomTextCliUtils.ApplicationLayer.Services.Chunker;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using Xunit;

namespace CustomTextCliUtils.Tests.UnitTests.ApplicationLayer.Services.Chunker
{
    public class MsReadChunkerServiceTest
    {
        public static TheoryData NoChunkingTestData()
        {
            string inputString1 = File.ReadAllText(@"TestData\Chunker\loremipsum-4.json");
            MsReadParseResult testInput1 = JsonConvert.DeserializeObject<MsReadParseResult>(inputString1);
            string expectedString1 = File.ReadAllText(@"TestData\Chunker\NoChunking\loremipsum-4_chunks.json");
            IEnumerable<ChunkInfo> output1 = JsonConvert.DeserializeObject<IEnumerable<ChunkInfo>>(expectedString1);
            return new TheoryData<ParseResult, IEnumerable<ChunkInfo>>
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
            MsReadParseResult testInput1 = JsonConvert.DeserializeObject<MsReadParseResult>(inputString1);
            string expectedString1 = File.ReadAllText(@"TestData\Chunker\PageChunking\loremipsum-4_chunks.json");
            IEnumerable<ChunkInfo> output1 = JsonConvert.DeserializeObject<IEnumerable<ChunkInfo>>(expectedString1);
            return new TheoryData<ParseResult, IEnumerable<ChunkInfo>>
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
            MsReadParseResult testInput1 = JsonConvert.DeserializeObject<MsReadParseResult>(inputString1);
            string expectedString1 = File.ReadAllText(@"TestData\Chunker\CharChunking\loremipsum-4_chunks.json");
            IEnumerable<ChunkInfo> output1 = JsonConvert.DeserializeObject<IEnumerable<ChunkInfo>>(expectedString1);
            return new TheoryData<ParseResult, IEnumerable<ChunkInfo>>
            {
                {
                    testInput1,
                    output1
                }
            };
        }

        [Theory]
        [MemberData(nameof(NoChunkingTestData))]
        public void NoChunkingTest(ParseResult parseResult, List<ChunkInfo> expectedChunks)
        {
            IChunkerService msReadChunker = new MsReadChunkerService();
            List<ChunkInfo> actualChunks = msReadChunker.Chunk(parseResult, ChunkMethod.NoChunking, 0);
            Assert.Equal(expectedChunks.Count, actualChunks.Count);
            Assert.Equal(expectedChunks, actualChunks, new ChunkInfoComparer());
        }

        [Theory]
        [MemberData(nameof(PageChunkingTestData))]
        public void PageChunkingTest(ParseResult parseResult, List<ChunkInfo> expectedChunks)
        {
            IChunkerService msReadChunker = new MsReadChunkerService();
            List<ChunkInfo> actualChunks = msReadChunker.Chunk(parseResult, ChunkMethod.Page, 0);
            Assert.Equal(expectedChunks.Count, actualChunks.Count);
            Assert.Equal(expectedChunks, actualChunks, new ChunkInfoComparer());
        }

        [Theory]
        [MemberData(nameof(CharChunkingTestData))]
        public void CharChunkingTest(ParseResult parseResult, List<ChunkInfo> expectedChunks)
        {
            IChunkerService msReadChunker = new MsReadChunkerService();
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
