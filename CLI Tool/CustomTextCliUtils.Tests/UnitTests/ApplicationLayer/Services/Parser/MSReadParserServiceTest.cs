using Xunit;
using CustomTextCliUtils.ApplicationLayer.Services.Parser;
using CustomTextCliUtils.ApplicationLayer.Exceptions;
using CustomTextCliUtils.ApplicationLayer.Exceptions.Parser;
using System;
using CustomTextCliUtils.Tests.Configs;

namespace CustomTextCliToo.Tests
{
    public class MSReadParserServiceTest
    {
        // Test Connection to MSRead Cognitive Service
        // ######################################################################
        public static TheoryData TestConnectionData()
        {
            return new TheoryData<string, string, bool>
            {
                {
                    // correct data 
                    Secrets.MSReadCognitiveServiceEndPoint,
                    Secrets.MSReadCongnitiveServiceKey,
                    true
                },
                {
                    // wrong service key
                    "https://eastus.api.cognitive.microsoft.com/",
                    "2e107685ed3e46bebd3ca123422b8f6b",
                    false
                },
                {
                    // wrong endpoint
                    "https://westus.api.cognitive.microsoft.com/",
                    "2e10bb66ed3e46bebd3ca1b3522b8f6b",
                    false
                }
            };
        }

        [Theory]
        [MemberData(nameof(TestConnectionData))]
        public void TestConnection(string cognitiveServiceEndPoint, string congnitiveServiceKey, bool correctParams)
        {
            if (correctParams)
            {
                var parser = new MSReadParserService(cognitiveServiceEndPoint, congnitiveServiceKey);
            }
            else {
                Assert.Throws<MsReadConnectionException>(() => {
                    new MSReadParserService(cognitiveServiceEndPoint, congnitiveServiceKey);
                });
            }
        }


        // Test Connection to MSRead Cognitive Service
        // ######################################################################
    }
}


