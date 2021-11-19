using Elsa.SKS.Package.ServiceAgents.Entities;

namespace Elsa.SKS.Package.ServiceAgents.Interfaces
{
    public interface IGeocodingAgent
    {
        public Geolocation GeocodeAddress(Address address);
    }
}