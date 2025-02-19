using CorrelationId.Abstractions;

namespace Logiwa.Web.Infrastructure.DelegatingHandlers
{
    public class CorrelationIdDelegatingHandler : DelegatingHandler
    {
        private const string HeaderName = "x-correlationid";
        private readonly ICorrelationContextAccessor _correlationContextAccessor;

        public CorrelationIdDelegatingHandler(ICorrelationContextAccessor correlationContextAccessor)
        {
            _correlationContextAccessor = correlationContextAccessor;
        }

        public CorrelationIdDelegatingHandler(ICorrelationContextAccessor correlationContextAccessor, HttpMessageHandler innerHandler) : base(innerHandler)
        {
            _correlationContextAccessor = correlationContextAccessor;
        }

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            if (!request.Headers.Contains(HeaderName))
                request.Headers.Add(HeaderName,
                    _correlationContextAccessor?.CorrelationContext?.CorrelationId ?? Guid.NewGuid().ToString());
            return base.SendAsync(request, cancellationToken);
        }
    }
}