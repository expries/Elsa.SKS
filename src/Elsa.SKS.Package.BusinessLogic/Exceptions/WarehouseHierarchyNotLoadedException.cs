using System;

namespace Elsa.SKS.Package.BusinessLogic.Exceptions
{
    public class WarehouseHierarchyNotLoadedException : BusinessException
    {
        public WarehouseHierarchyNotLoadedException()
        {
        }

        public WarehouseHierarchyNotLoadedException(string message) : base(message)
        {
        }

        public WarehouseHierarchyNotLoadedException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}