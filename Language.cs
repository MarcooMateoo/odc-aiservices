using OutSystems.ExternalLibraries.SDK;

namespace AzureAIServices
{
    [OSStructure(Description = "Represents an languages that can be use in AI Translate")]
    public struct Language
    {
        [OSStructureField(Description = "language code that can use in AI translate methods", IsMandatory = true)]
        public string Code;
        [OSStructureField(Description = "Name of the language", IsMandatory = true)]
        public string Name;
    }
}
