using System;

namespace Elsa.SKS.Package.BusinessLogic.Exceptions
{
    public class BusinessException : Exception
    {
        protected BusinessException()
        {
        }

        protected BusinessException(string message) : base(message)
        {
        }

        protected BusinessException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}