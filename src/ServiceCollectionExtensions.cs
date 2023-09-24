using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Gulla.Episerver.Labs.LanguageManager.OpenAi
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddLanguageManagerOpenAi(this IServiceCollection services)
        {
            return AddLanguageManagerOpenAi(services, _ => { });
        }

        public static IServiceCollection AddLanguageManagerOpenAi(this IServiceCollection services, Action<LanguageManagerOpenAiOptions> setupAction)
        {
            services.AddTransient<OpenAiService, OpenAiService>();
            
            services.AddOptions<LanguageManagerOpenAiOptions>().Configure<IConfiguration>((options, configuration) =>
            {
                setupAction(options);
                configuration.GetSection("Gulla:LanguageManagerOpenAi").Bind(options);
            });

            return services;
        }
    }
}