using System;

namespace Elsa.SKS.Package.BusinessLogic.Exceptions
{
    public class InvalidParcelWeightException : BusinessException
    {
        public InvalidParcelWeightException()
        {
        }

        public InvalidParcelWeightException(string message) : base(message)
        {
        }

        public InvalidParcelWeightException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}