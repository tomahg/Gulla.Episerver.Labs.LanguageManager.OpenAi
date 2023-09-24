namespace Gulla.Episerver.Labs.LanguageManager.OpenAi
{
    public class LanguageManagerOpenAiOptions
    {
        public string? OpenAiApiKey { get; set; }

        public string OpenAiModel { get; set; } = "gpt-3.5-turbo";

        public double OpenAiTemperature { get; set; } = 0.7;

        public string? OpenAiExtraPrompt { get; set; }
    }
}
