using System;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Backend.BusinessLogic.Exceptions
{
    [ExcludeFromCodeCoverage]
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