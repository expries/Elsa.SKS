using System;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Backend.BusinessLogic.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class TrackingException : BusinessException
    {
        public TrackingException()
        {
        }

        public TrackingException(string message) : base(message)
        {
        }

        public TrackingException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}