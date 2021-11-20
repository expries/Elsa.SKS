using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Package.ServiceAgents.Entities
{
    [ExcludeFromCodeCoverage]
    public class Address
    {
        public string? Street { get; set; }
        
        public string? City { get; set; }
        
        public string? Query { get; set; }
    }
}