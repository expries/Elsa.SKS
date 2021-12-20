using System;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Backend.BusinessLogic.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class SubscriptionNotFoundException : BusinessException
    {
        public SubscriptionNotFoundException()
        {
        }

        public SubscriptionNotFoundException(string message) : base(message)
        {
        }

        public SubscriptionNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}