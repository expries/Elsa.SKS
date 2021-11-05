using System;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Package.BusinessLogic.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ParcelNotFoundException : BusinessException
    {
        public ParcelNotFoundException()
        {
        }

        public ParcelNotFoundException(string message) : base(message)
        {
        }

        public ParcelNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}