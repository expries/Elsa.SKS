using Elsa.SKS.Backend.ServiceAgents.Entities;

namespace Elsa.SKS.Backend.ServiceAgents.Interfaces
{
    public interface IGeocodingAgent
    {
        public Geolocation GeocodeAddress(Address address);
    }
}