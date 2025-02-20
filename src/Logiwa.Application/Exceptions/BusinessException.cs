using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.Logging;

namespace Logiwa.Application.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class BusinessException : Exception
    {
        public BusinessException(string message, EventId eventId = default, string userFriendlyMessage = "",
            string code = "500") : base(message, null)
        {
            Code = code;
            UserFriendlyMessage = userFriendlyMessage;
            EventId = eventId;
        }

        public string Code { get; }
        public string UserFriendlyMessage { get; }
        public EventId EventId { get; }
    }
}