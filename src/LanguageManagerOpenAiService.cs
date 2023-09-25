using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Gulla.Episerver.Labs.LanguageManager.OpenAi
{
    public class LanguageManagerOpenAiService
    {
        private readonly IOptions<LanguageManagerOpenAiOptions> _options;
        private readonly string _endpointCompletions = "https://api.openai.com/v1/chat/completions";

        public LanguageManagerOpenAiService(IOptions<LanguageManagerOpenAiOptions> options)
        {
            if (options.Value.OpenAiApiKey == null)
            {
                throw new ArgumentException("Missing OpenAI API Key for Gulla.Episerver.Labs.LanguageManager.OpenAi!");
            }

            _options = options;            
        }

        /// <summary>
        /// Translate text
        /// </summary>
        /// <param name="input">The text to translate</param>
        /// <param name="fromLanguageName">The name of the source language</param>
        /// <param name="toLanguageName">The name of the destination language</param>
        /// <returns></returns>
        public async Task<string> TranslateText(string input, string fromLanguageName, string toLanguageName)
        {
            using var httpClient = new HttpClient();
            httpClient.DefaultRequestHeaders.Add("Authorization", $"Bearer {_options.Value.OpenAiApiKey}");

            var prompt = $"Translate the following content from {fromLanguageName} to {toLanguageName}.";
            if (!string.IsNullOrEmpty(_options.Value.OpenAiExtraPrompt))
            {
                prompt += _options.Value.OpenAiExtraPrompt;
            }
            prompt += $"\r\n\r\n{input}";

            var request = new HttpRequestMessage(HttpMethod.Post, _endpointCompletions);
            var messages = new []{ new{role = "user", content = prompt } };
            var content = new StringContent(JsonConvert.SerializeObject(new { messages = messages, model = _options.Value.OpenAiModel, temperature = _options.Value.OpenAiTemperature }), Encoding.UTF8, "application/json");
            request.Content = content;

            var response = await httpClient.SendAsync(request);
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<dynamic>(json);

            var errorMessage = data.error?.message?.ToString();
            if (!string.IsNullOrEmpty(errorMessage))
            {
                throw new Exception(errorMessage);
            }

            var text = data?.choices[0]?.message?.content?.ToString();
            if (!string.IsNullOrEmpty(text))
            {
                return text ?? "";
            }

            return "Error generating AI text";
        }
    }
}
