using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoManager.Application.ApplicationService;
using VideoManager.Application.Configuration;
using VideoManager.Helpers;
using VideoManager.Infrastructure;

namespace VideoManager.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
        {
            services
                .AddSingleton(configuration.FetchConfiguration<VideoManagerCfg>())
                .AddTransient<IApplicationService, ApplicationService.ApplicationService>()
                .AddInfrastructureLayer(configuration)
                .AddAutoMapper(typeof(ApplicationMappingProfile), typeof(InfrastructureMappingProfile));
            return services;
        }
    }
}
