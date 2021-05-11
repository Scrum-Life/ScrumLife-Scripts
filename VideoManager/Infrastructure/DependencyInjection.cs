using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Polly;
using Polly.Extensions.Http;
using System;
using System.Net.Http.Headers;
using VideoManager.Domain.Interfaces;
using VideoManager.Helpers;
using VideoManager.Infrastructure.Airtable;
using VideoManager.Infrastructure.Amara;
using VideoManager.Infrastructure.GitHub;
using VideoManager.Infrastructure.YouTube;

namespace VideoManager.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructureLayer(this IServiceCollection services, IConfiguration configuration)
        {
            AirtableConfiguration airtableConfiguration;
            AmaraConfiguration amaraConfiguration;
            GithubConfiguration githubConfiguration;
            YoutubeConfiguration youtubeConfiguration;

            airtableConfiguration = configuration.FetchConfiguration<AirtableConfiguration>();
            amaraConfiguration = configuration.FetchConfiguration<AmaraConfiguration>();
            githubConfiguration = configuration.FetchConfiguration<GithubConfiguration>();
            youtubeConfiguration = configuration.FetchConfiguration<YoutubeConfiguration>();
            services.AddSingleton(airtableConfiguration);
            services.AddSingleton(amaraConfiguration);
            services.AddSingleton(githubConfiguration);
            services.AddSingleton(youtubeConfiguration);
            services.AddSingleton<YoutubeServiceProvider>();

            services.AddHttpClient(nameof(AmaraAdapter), c =>
            {
                c.BaseAddress = amaraConfiguration.BaseUrl;
                c.DefaultRequestHeaders.Clear();
                c.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                c.DefaultRequestHeaders.Add("Token", amaraConfiguration.ApiKey);
            }).AddPolicyHandler(HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.BadGateway)
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.GatewayTimeout)
                .OrResult(msg => msg.StatusCode == System.Net.HttpStatusCode.RequestTimeout)
                .WaitAndRetryAsync(3,
                    retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)) + TimeSpan.FromMilliseconds(new Random().Next(0, 100))
                    )
            );

            services.AddTransient<IDataRepositoryAdapter, AirtableClient>();
            services.AddTransient<ISubtitleAdapter, AmaraAdapter>();
            services.AddTransient<GithubClient>();
            services.AddTransient<IVideoAdapter, YoutubeAdapter>();

            return services;
        }
    }
}
