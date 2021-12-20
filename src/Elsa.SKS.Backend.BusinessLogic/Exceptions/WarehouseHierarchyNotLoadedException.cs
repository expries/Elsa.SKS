using System;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Backend.BusinessLogic.Exceptions
{
    [ExcludeFromCodeCoverage]
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