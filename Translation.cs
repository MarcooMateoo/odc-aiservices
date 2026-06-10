using OutSystems.ExternalLibraries.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureAIServices
{
    [OSStructure(Description = "Represents the translated language after running AI Translate")]
    public struct Translation
    {
        [OSStructureField(Description = "source language", IsMandatory = false)]
        public string Source;
        [OSStructureField(Description = "Confidence Score", IsMandatory = false)]
        public float ConfidenceScore;
        [OSStructureField(Description = "target language", IsMandatory = false)]
        public string Target;
        [OSStructureField(Description = "Translated Text", IsMandatory = false)]
        public string TranslatedText;
    }
}
