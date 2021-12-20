using System;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Backend.BusinessLogic.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class InvalidParcelException : BusinessException
    {
        public InvalidParcelException()
        {
        }

        public InvalidParcelException(string message) : base(message)
        {
        }

        public InvalidParcelException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}