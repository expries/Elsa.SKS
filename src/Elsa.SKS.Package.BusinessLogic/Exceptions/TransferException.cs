using System;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Package.BusinessLogic.Exceptions
{
    [ExcludeFromCodeCoverage]
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