using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OutSystems.ExternalLibraries.SDK;

namespace AzureAIServices
{
    [OSInterface]
    public interface IAzureAIServices
    {

        /*Azure AI Translate Nuget - https://ai.azure.com/doc/azure/ai-services/reference/sdk-package-resources?pivots=programming-language-python&tid=f0abb1ad-9380-4867-a1c1-bac62e7b3450*/
        [OSAction(Description = "Retrieves all supported language", ReturnName = "Languages")]
        public List<Language> GetLanguages(
            [OSParameter (Description = "API key are generated in from Azure AI Foundry ")]
            string APIKey,
            [OSParameter (Description = "Region are generated in from Azure AI Foundry")]
            string Region);


        [OSAction(Description = "Synchronously Translates Text", ReturnName = "Translated", ReturnType = OSDataType.Text)]
        public Translation Translate(
            [OSParameter (Description = "API key are generated in from Azure AI Foundry ")]
            string APIKey,
            [OSParameter (Description = "Region are generated in from Azure AI Foundry ")]
            string Region,
            [OSParameter (Description = "Text to be translated")]
            string Text,
            [OSParameter (Description = "Language code of the target translation. Use GetLanguages for the supported code")]
            IEnumerable<string> Target,
            [OSParameter (Description = "Language code of the source. Use GetLanguages for the supported code. By default it is set to English (en)")]
            string Source = "en",
            [OSParameter (Description = "Accepts 'plain' (default) or 'html'")]
            string TextType = "plain");


        [OSAction(Description = "Synchronously Extracts Document Content as Markdown. This uses the built in models in Azure AI Document", ReturnName = "MarkdownContent", ReturnType = OSDataType.Text)]
        public string ReadDocumentAsMarkdown(
            [OSParameter (Description = "API key are generated in from Azure AI Foundry")]
            string APIKey,
            [OSParameter (Description = "Endpoint generated from Azure AI Foundry. ")]
            string Endpoint,
            [OSParameter (Description = "Document to be read and processed by Azure Document AI")]
            byte[] File);


        [OSAction(Description = "Chats with AI Agents deployed in Azure AI Foundry", ReturnName = "Response", ReturnType = OSDataType.Text)]
        public OpenAIResult OpenAIChat(
            [OSParameter (Description = "API key are generated in from Azure AI Foundry")]
            string APIKey,
            [OSParameter (Description = "Endpoint generated from Azure AI Foundry. ")]
            string Endpoint,
            [OSParameter (Description = "Deployment Name of the model from Azure AI Foundry")]
            string DeploymentName,
            [OSParameter (Description = "Messages that will be processed by OpenAI")]
            List<OpenAIMessage> Messages,
            [OSParameter (Description = "Controls randomness in the response, use lower to be more deterministic.")]
            float Temperature = 1.0f,
            [OSParameter (Description = "Limit the maximum output tokens for the model response.")]
            int MaxOutputToken = 1000
        );
    }
}
