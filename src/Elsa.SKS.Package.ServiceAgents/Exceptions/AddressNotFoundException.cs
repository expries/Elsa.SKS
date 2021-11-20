using System;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Package.ServiceAgents.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class AddressNotFoundException : ServiceAgentException
    {
        public AddressNotFoundException()
        {
        }

        public AddressNotFoundException(string message) : base(message)
        {
        }

        public AddressNotFoundException(string message, Exception inner) : base(message, inner)
        {
        }
    }

}