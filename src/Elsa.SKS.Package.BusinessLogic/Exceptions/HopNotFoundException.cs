using System;

namespace Elsa.SKS.Package.BusinessLogic.Exceptions
{
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