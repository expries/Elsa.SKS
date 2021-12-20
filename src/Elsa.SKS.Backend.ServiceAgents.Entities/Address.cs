using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Backend.ServiceAgents.Entities
{
    [ExcludeFromCodeCoverage]
    public class Address
    {
        public string? Street { get; set; }

        public string? PostalCode { get; set; }

        public string? City { get; set; }

        public string? Country { get; set; }
    }
}