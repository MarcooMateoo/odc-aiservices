using OutSystems.ExternalLibraries.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAIServices
{
    [OSStructure(Description = "OpenAI Response")]
    public struct OpenAIResult
    {
        [OSStructureField(Description = "Response of the Agent", IsMandatory = true)]
        public string Message;

        [OSStructureField(Description = "The total number of combined input (prompt) and output (completion) tokens used by a chat completion operation", IsMandatory = true)]
        public int TotalTokenCount;

        [OSStructureField(Description = "The number of tokens in the request message input, spanning all message content items.", IsMandatory = true)]
        public int TotalInputToken;

        [OSStructureField(Description = "The combined number of output tokens in the generated completion, as consumed by the model.", IsMandatory = true)]
        public int TotalOutputToken;
    }
}
