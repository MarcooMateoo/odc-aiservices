using Azure;
using Azure.AI.DocumentIntelligence;
using Azure.AI.OpenAI;
using Azure.AI.Translation.Text;
using OpenAI;
using OpenAI.Chat;
using OutSystems.ExternalLibraries.SDK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AzureAIServices
{
    public class AzureAIServices : IAzureAIServices
    {
        #region GetLanguages
        [OSAction(Description = "Retrieves all supported language", ReturnName = "Languages")]
        public List<Language> GetLanguages(
            [OSParameter (Description = "API key are generated in from Azure Portal Resource. ")]
            string APIKey,
            [OSParameter (Description = "Region are generated in from Azure Portal Resource. ")]
            string Region)
        {

            AzureKeyCredential credential = new(APIKey);
            TextTranslationClient client = new(credential, Region);
            var languages = new List<Language>();

            Response<GetSupportedLanguagesResult> response = client.GetSupportedLanguages();
            GetSupportedLanguagesResult langs = response.Value;

            foreach (var translationLanguage in langs.Translation)
            {
                Language lang = new();
                lang.Code = translationLanguage.Key;
                lang.Name = translationLanguage.Value.Name;

                languages.Add(lang);
            }

            return languages;
        }
        #endregion

        #region Translate
        [OSAction(Description = "Synchronously Translates Text", ReturnName = "Translated", ReturnType = OSDataType.Text)]
        public Translation Translate(
            [OSParameter (Description = "API key are generated in from Azure Portal Resource. ")]
            string APIKey,
            [OSParameter (Description = "Region are generated in from Azure Portal Resource. ")]
            string Region,
            [OSParameter (Description = "Text to be translated")]
            string Text,
            [OSParameter (Description = "Language code of the target translation. Use GetLanguages for the supported code")]
            IEnumerable<string> Target,
            [OSParameter (Description = "Language code of the source. Use GetLanguages for the supported code. By default it is set to English (en)")]
            string Source = "en",
            [OSParameter (Description = "Accepts 'plain' (default) or 'html'")]
            string TextType = "plain")
        {

            AzureKeyCredential credential = new(APIKey);
            TextTranslationClient client = new(credential, Region);

            IEnumerable<string> inputTextElements = new[]{
                    Text
            };

            var translated = new Translation();

            Response<IReadOnlyList<TranslatedTextItem>> response = client.Translate(Target, inputTextElements, sourceLanguage: Source, textType: TextType);
            IReadOnlyList<TranslatedTextItem> translations = response.Value;
            TranslatedTextItem translation = translations.FirstOrDefault();

            translated.Source = translation?.DetectedLanguage?.Language;
            if (translation?.DetectedLanguage?.Confidence > 0)
            {
                translated.ConfidenceScore = (float)translation?.DetectedLanguage?.Confidence;
            }
            else
            {
                translated.ConfidenceScore = 0;
            }
            translated.TranslatedText = translation?.Translations?.FirstOrDefault()?.Text;
            translated.Target = translation?.Translations?.FirstOrDefault().TargetLanguage;
            
            return translated;
        }
        #endregion

        #region ReadDocumentAsMarkdown
        public string ReadDocumentAsMarkdown(
            [OSParameter (Description = "API key are generated in from Azure AI Foundry")]
            string APIKey,
            [OSParameter (Description = "Endpoint generated from Azure AI Foundry. ")]
            string Endpoint,
            [OSParameter (Description = "Document to be read and processed by Azure Document AI")]
            byte[] File)
        {

            var client = new DocumentIntelligenceClient(new Uri(Endpoint), new AzureKeyCredential(APIKey));

            var options = new AnalyzeDocumentOptions("prebuilt-layout", BinaryData.FromBytes(File))
            {
                OutputContentFormat = DocumentContentFormat.Markdown
            };

            return AnalyzeDocumentAsync(client, options).GetAwaiter().GetResult(); ;
        }

        private async Task<string> AnalyzeDocumentAsync(DocumentIntelligenceClient client, AnalyzeDocumentOptions options)
        {

            Operation<AnalyzeResult> operation = await client.AnalyzeDocumentAsync(WaitUntil.Completed, options);
            AnalyzeResult result = operation.Value;

            return result.Content;
        }

        #endregion

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
        )
        {
            ChatClient client = new(
            credential: new AzureKeyCredential(APIKey),
            model: DeploymentName,
            options: new OpenAIClientOptions()
            {
                Endpoint = new($"{Endpoint}"),
            });

            var chatOptions = new ChatCompletionOptions
            {
                Temperature = Temperature,
                MaxOutputTokenCount = MaxOutputToken,
                
            };

            var messages = new List<ChatMessage>();
            string airesponse = "";

            foreach (OpenAIMessage message in Messages)
            {
                if (message.Role.ToLower() == "system") { messages.Add(new SystemChatMessage(message.Message)); }
                if (message.Role.ToLower() == "assistant") { messages.Add(new AssistantChatMessage(message.Message)); }
                if (message.Role.ToLower() == "user") { messages.Add(new UserChatMessage(message.Message)); }
            }

            ChatCompletion completion = client.CompleteChat(messages, chatOptions);


            foreach (ChatMessageContentPart contentPart in completion.Content)
            {
                airesponse = contentPart.Text;
            }

            return new OpenAIResult()
            {
                Message = airesponse,
                TotalTokenCount = completion.Usage.TotalTokenCount,
                TotalInputToken = completion.Usage.InputTokenCount,
                TotalOutputToken = completion.Usage.OutputTokenCount
            };
        }
    }
}
