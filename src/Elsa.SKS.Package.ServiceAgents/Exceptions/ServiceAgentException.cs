using System;
using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Package.ServiceAgents.Exceptions
{
    [ExcludeFromCodeCoverage]
    public class ServiceAgentException : Exception
    {
        public ServiceAgentException()
        {
        }

        public ServiceAgentException(string message) : base(message)
        {
        }

        public ServiceAgentException(string message, Exception inner) : base(message, inner)
        {
        }
    }
    
}