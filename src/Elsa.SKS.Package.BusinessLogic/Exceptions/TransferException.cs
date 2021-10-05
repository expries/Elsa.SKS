using System;

namespace Elsa.SKS.Package.BusinessLogic.Exceptions
{
    public class TransferException : BusinessException
    {
        public TransferException()
        {
        }

        public TransferException(string message) : base(message)
        {
        }

        public TransferException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}