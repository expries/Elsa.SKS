using System;

namespace Elsa.SKS.Package.BusinessLogic.Exceptions
{
    public class InvalidWarehouseException : BusinessException
    {
        public InvalidWarehouseException()
        {
        }

        public InvalidWarehouseException(string message) : base(message)
        {
        }

        public InvalidWarehouseException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}