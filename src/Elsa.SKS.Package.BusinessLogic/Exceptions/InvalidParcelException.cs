using System;

namespace Elsa.SKS.Package.BusinessLogic.Exceptions
{
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