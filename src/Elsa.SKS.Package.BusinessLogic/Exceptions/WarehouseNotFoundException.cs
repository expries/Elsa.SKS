using System;

namespace Elsa.SKS.Package.BusinessLogic.Exceptions
{
    public class WarehouseNotFoundException : BusinessException
    {
        public WarehouseNotFoundException()
        {
        }

        public WarehouseNotFoundException(string message) : base(message)
        {
        }

        public WarehouseNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}