using System;
using System.Net.Http;
using Elsa.SKS.Backend.BusinessLogic.Entities;
using Elsa.SKS.Backend.ServiceAgents.Exceptions;
using Elsa.SKS.Backend.ServiceAgents.Interfaces;
using Microsoft.Extensions.Logging;

namespace Elsa.SKS.Backend.ServiceAgents
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
                _logger.LogError(ex, "Failed to contact logistics partner.");
                throw new ServiceAgentException("Failed to contact logistics partner.", ex);
            }
        }
    }
}