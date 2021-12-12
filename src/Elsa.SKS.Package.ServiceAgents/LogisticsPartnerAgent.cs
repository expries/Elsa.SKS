using System;
using System.Net.Http;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.ServiceAgents.Exceptions;
using Elsa.SKS.Package.ServiceAgents.Interfaces;
using Microsoft.Extensions.Logging;

namespace Elsa.SKS.Package.ServiceAgents
{
    public class LogisticsPartnerAgent : ILogisticsPartnerAgent
    {
        private readonly HttpClient _client;
        
        private readonly ILogger<LogisticsPartnerAgent> _logger;
        
        public LogisticsPartnerAgent(HttpClient client, ILogger<LogisticsPartnerAgent> logger)
        {
            _client = client;
            _logger = logger;
        }
        
        public void TransferParcel(TransferWarehouse warehouse, Parcel parcel)
        {
            try
            {
                string url = $"{warehouse.LogisticsPartnerUrl}/parcel/{parcel.TrackingId}";
                var content = new StringContent(string.Empty);
                _client.PostAsync(url, content).Wait();
            }
            catch (Exception ex)
            {
                if (ex is HttpRequestException || ex is InvalidOperationException)
                {
                    throw new ServiceAgentException("Request error occured.", ex);

                }
            }
        }
    }
}