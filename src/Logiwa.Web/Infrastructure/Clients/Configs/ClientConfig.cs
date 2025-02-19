using System.Diagnostics.CodeAnalysis;

namespace Logiwa.Web.Infrastructure.Clients.Configs
{
    [ExcludeFromCodeCoverage]
    public class ClientConfig
    {
        public string BaseAddress { get; set; }
        public int Timeout { get; set; }
        public int RetryCount { get; set; }
        public int RetryDelayInMs { get; set; }
    }
}