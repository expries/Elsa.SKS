using System;

namespace Elsa.SKS.Package.BusinessLogic.Exceptions
{
    public class ParcelNotFoundException : BusinessException
    {
        protected ParcelNotFoundException()
        {
        }

        protected ParcelNotFoundException(string message) : base(message)
        {
        }

        protected ParcelNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}