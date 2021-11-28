using System.Net.Http;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.ServiceAgents.Interfaces;

namespace Elsa.SKS.Package.ServiceAgents
{
    public class LogisticsPartnerAgent : ILogisticsPartnerAgent
    {
        private readonly HttpClient _client;
        
        public LogisticsPartnerAgent(HttpClient client)
        {
            _client = client;
        }
        
        public void TransferParcel(TransferWarehouse warehouse, Parcel parcel)
        {
            string url = $"{warehouse.LogisticsPartnerUrl}/parcel/{parcel.TrackingId}";
            var content = new StringContent(string.Empty);
            _client.PostAsync(url, content).Wait();
        }
    }
}