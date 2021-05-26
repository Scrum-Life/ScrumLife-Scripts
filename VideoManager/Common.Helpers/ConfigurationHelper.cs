using Microsoft.Extensions.Configuration;
using System;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace VideoManager.Helpers
{
    public static class ConfigurationHelper
    {
        public static TConfiguration FetchConfiguration<TConfiguration>(this IConfiguration configuration) where TConfiguration : new()
        {
            TConfiguration configurationObject;
            try
            {
                configurationObject = configuration
                    .GetSection(typeof(TConfiguration).Name)
                    .Get<TConfiguration>();
                
                CheckIfConfigurationObjectStringPropsAreSetOrThrow(configurationObject);
            }
            catch (InvalidOperationException e)
            {
                throw new MissingConfigurationException($"Missing configuration in {typeof(TConfiguration).Name}", e);
            }

            return configurationObject ?? new TConfiguration();
        }
        private static void CheckIfConfigurationObjectStringPropsAreSetOrThrow<TConfiguration>(TConfiguration sut)
        {
            Type type = typeof(TConfiguration);
            string[] result = type.GetProperties()
                .Where(p => Attribute.IsDefined(p, typeof(RequiredAttribute)))
                .Where(p => p.PropertyType == typeof(string) && string.IsNullOrWhiteSpace(p.GetValue(sut) as string)
                            || p.PropertyType == typeof(Guid) && (p.GetValue(sut) is null || ((Guid) (p.GetValue(sut) ?? Guid.Empty)).Equals(Guid.Empty))
                            || p.PropertyType == typeof(Uri) && string.IsNullOrWhiteSpace((p.GetValue(sut) as Uri)?.ToString()))
                .Select(o => o.Name)
                .ToArray();

            if (result.Length > 0)
            {
                throw new MissingConfigurationException($"Object {type.Name} has some empty properties: {string.Join(", ", result)}. Please check appsettings.json file or environment variables.");
            }
        }
    }
}
