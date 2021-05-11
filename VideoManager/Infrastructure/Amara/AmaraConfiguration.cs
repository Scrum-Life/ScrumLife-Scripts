using System;

namespace VideoManager.Infrastructure.Amara
{
    internal sealed class AmaraConfiguration
    {
        public string ApiKey { get; set; }

        public Uri BaseUrl { get; set; }
    }
}
