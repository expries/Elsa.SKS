using System;

namespace Elsa.SKS.Package.BusinessLogic.Exceptions
{
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