using System.Diagnostics.CodeAnalysis;

namespace Elsa.SKS.Backend.ServiceAgents.Entities
{
    [ExcludeFromCodeCoverage]
    public class Geolocation
    {
        public double Latitude { get; set; }

        public double Longitude { get; set; }
    }
}