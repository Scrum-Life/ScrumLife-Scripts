using Domain.Data;
using Domain.Video;
using Microsoft.Extensions.DependencyInjection;

namespace Domain
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDomainLayer(this IServiceCollection services)
        {
            services.AddTransient<IVideoService, VideoService>();
            services.AddTransient<IDataAccessorService, DataAccessorService>();

            return services;
        }
    }
}
