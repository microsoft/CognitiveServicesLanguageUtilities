// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.
using Microsoft.IAPUtilities.Definitions.APIs.Services;
using Microsoft.IAPUtilities.Definitions.Enums.IAP;
using Microsoft.IAPUtilities.Definitions.Exceptions;
using Microsoft.IAPUtilities.Definitions.Models.IAP;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Microsoft.IAPUtilities.Core.Services.IAP
{
    public class TranscriptParser : ITranscriptParser
    {
        public async Task<IAPTranscript> ParseTranscriptAsync(Stream file)
        {
            var sr = new StreamReader(file);

            // Extract meta data from first line in transcript
            var firstLine = await sr.ReadLineAsync();
            ExtractMetaData(firstLine, out string id, out string channel);

            // Read every line into an utterance
            string line;
            var utterances = new List<ConversationUtterance>();
            while ((line = await sr.ReadLineAsync()) != null)
            {
                utterances.Add(ParseLine(line));
            }

            return new IAPTranscript
            {
                Channel = (ChannelType)int.Parse(channel),
                Id = id,
                Utterances = utterances
            };
        }

        private static void ExtractMetaData(string firstLine, out string id, out string channel)
        {
            var pattern = new Regex(@"Id:(?<Id>\d+) Channel:(?<Channel>\d+)", RegexOptions.IgnoreCase);
            var match = pattern.Match(firstLine);
            if (match.Success)
            {
                id = match.Groups["Id"].Value;
                channel = match.Groups["Channel"].Value;
            }
            else
            {
                throw new CliException("Invalid Id or Channel Format");
            }
        }

        private ConversationUtterance ParseLine(string line)
        {
            var timestampIndex = line.IndexOf(':');
            var timestamp = long.Parse(line.Substring(0, timestampIndex));
            return new ConversationUtterance
            {
                Timestamp = timestamp,
                Text = line.Substring(timestampIndex + 1).Trim()
            };
        }
    }
}
