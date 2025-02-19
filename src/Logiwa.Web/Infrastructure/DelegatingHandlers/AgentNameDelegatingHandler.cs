namespace Logiwa.Web.Infrastructure.DelegatingHandlers
{
    public class AgentNameDelegatingHandler : DelegatingHandler
    {
        private static readonly string _headerName = "x-agentname";

        public AgentNameDelegatingHandler()
        {
        }

        public AgentNameDelegatingHandler(HttpMessageHandler innerHandler) : base(innerHandler)
        {
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(_headerName))
                request.Headers.Add(_headerName, new List<string> {"product-api"});

            return base.SendAsync(request, cancellationToken);
        }
    }
}