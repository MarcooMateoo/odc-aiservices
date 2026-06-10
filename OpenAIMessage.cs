using OutSystems.ExternalLibraries.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAIServices
{
    [OSStructure(Description = "Represents messages that will be processed in OpenAI Chat")]
    public struct OpenAIMessage
    {
        [OSStructureField(Description = "Identifies the sender of the message. It accepts \"user\" if it's a message sent by a user, \"assistant\" if it's a response from the AI model, and \"system\" if it's the initial instruction prompt.", IsMandatory = true)]
        public string Role;
        [OSStructureField(Description = "The content of the message.", IsMandatory = true)]
        public string Message;
    }
}
