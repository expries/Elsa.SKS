using System;

namespace Elsa.SKS.Package.BusinessLogic.Exceptions
{
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