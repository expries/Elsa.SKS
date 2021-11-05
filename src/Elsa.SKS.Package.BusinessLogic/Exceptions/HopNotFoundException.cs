using System;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Package.BusinessLogic.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class HopNotFoundException : BusinessException
    {
        public HopNotFoundException()
        {
        }

        public HopNotFoundException(string message) : base(message)
        {
        }

        public HopNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }
}