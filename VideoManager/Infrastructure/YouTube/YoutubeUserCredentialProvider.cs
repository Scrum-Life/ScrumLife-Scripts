using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using Google.Apis.YouTube.v3;
using Microsoft.Extensions.Logging;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace VideoManager.Infrastructure.YouTube
{
    public sealed class YoutubeServiceProvider
    {
        private readonly ILogger<YoutubeServiceProvider> _logger;
        private readonly YoutubeConfiguration _config;

        public YoutubeServiceProvider(ILogger<YoutubeServiceProvider> logger, YoutubeConfiguration config)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        private async Task<UserCredential> GetUserCredentialAsync(CancellationToken cancelationToken)
        {
            _logger.LogDebug($"GetTypeGetUserCredentialAsync for user {_config.Username}");
            try
            {
                return await GoogleWebAuthorizationBroker.AuthorizeAsync(
                        new ClientSecrets()
                        {
                            ClientId = _config.GoogleClientId,
                            ClientSecret = _config.GoogleClientSecret
                        },
                        new[] { 
                            YouTubeService.Scope.YoutubeForceSsl, 
                            YouTubeService.Scope.Youtube, 
                            YouTubeService.Scope.Youtubepartner 
                        },
                        _config.Username,
                        cancelationToken,
                        new FileDataStore(GetType().ToString())
                );
            }
            catch(Exception e)
            {
                _logger.LogError($"Unable to get YouTube credential object", e);
                throw;
            }
        }

        public async Task<YouTubeService> CreateServiceAsync(CancellationToken cancelationToken)
        {
            try
            {
                UserCredential credentials = await GetUserCredentialAsync(cancelationToken);
                
                return new YouTubeService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credentials,
                    ApplicationName = _config.AplicationName ?? "YTVideoManager",
                });
            }
            catch(Exception e)
            {
                _logger.LogError($"Unable to create YouTubeService object", e);
                throw;
            }
        }
    }
}
