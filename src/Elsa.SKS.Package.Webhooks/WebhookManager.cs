using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.DataAccess.Entities;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
using Elsa.SKS.Package.Webhooks.Interfaces;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using WebhookMessage = Elsa.SKS.Package.DataAccess.Entities.WebhookMessage;

namespace Elsa.SKS.Package.Webhooks
{
    public class WebhookManager : IWebhookManager
    {
        private readonly ISubscriberRepository _subscriberRepository;
        
        private readonly IParcelRepository _parcelRepository;

        private readonly IMapper _mapper;
        
        private readonly ILogger<WebhookManager> _logger;

        public WebhookManager(ISubscriberRepository subscriberRepository, IParcelRepository parcelRepository, IMapper mapper, ILogger<WebhookManager> logger)
        {
            _subscriberRepository = subscriberRepository;
            _parcelRepository = parcelRepository;
            _mapper = mapper;
            _logger = logger;
        }

        public Subscription AddSubscription(Subscription newSubscription)
        {
            try
            {
                // checks if parcel exists
                if (_parcelRepository.GetByTrackingId(newSubscription.TrackingId) == null)
                {
                    _logger.LogInformation("Parcel with tracking id was not found");
                    throw new ParcelNotFoundException($"Parcel with tracking id {newSubscription.TrackingId} was not found");
                }
                
                var subscription = _subscriberRepository.Create(newSubscription);
                return subscription;
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Database error");
                throw new DataAccessException("A database error has occurred.", ex);
            }
        }
        public bool DeleteAllSubscriptionsByTrackingId(string trackingId)
        {
            try
            {
                return _subscriberRepository.DeleteAllByTrackingId(trackingId);
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Database error");
                throw new DataAccessException("A database error has occurred.", ex);
            }

        }
        
        public bool DeleteSubscriptionById(long? id)
        {
            try
            {
                // checks if subscription id exists
                if (_subscriberRepository.GetById(id) == null)
                {
                    _logger.LogInformation("Subscription with this id was not found");
                    throw new SubscriptionNotFoundException($"Subscription with id {id} was not found");
                }
                return _subscriberRepository.Delete(id);

            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Database error");
                throw new DataAccessException("A database error has occurred.", ex);
            }
        }

        public List<Subscription> GetParcelWebhooks(string trackingId)
        {
            try
            {
                if (_subscriberRepository.GetByTrackingId(trackingId).ToList().Count == 0)
                {
                    _logger.LogInformation("Parcel with tracking id was not found");
                    throw new ParcelNotFoundException($"Parcel with tracking id {trackingId} was not found");
                }
                return _subscriberRepository.GetByTrackingId(trackingId).ToList();
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Database error");
                throw new DataAccessException("A database error has occurred.", ex);
            }

        }

        public async Task ConfirmRegistration(Subscription subscription)
        {
            using var client = new HttpClient();
            var clientAddress = new Uri(subscription.Url);
            client.BaseAddress = clientAddress;
            var response = await client.GetAsync(client.BaseAddress + $"/?echo={subscription.Id}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation("Two way handshake was successful");
            }
            var result = response.Content.ReadAsStringAsync().Result;
            _logger.LogInformation("Client response: " + result);
        }

        public async Task NotifySubscribers(WebhookMessage message)
        {
            var subscribers = _subscriberRepository.GetByTrackingId(message.TrackingId);
            foreach (var subscriber in subscribers)
            {
                // send message with parcel infos
                await SendParcelInfosToClient(message, subscriber.Url);
            }
        }

        private async Task SendParcelInfosToClient(WebhookMessage body, string subscriberUrl)
        {
            using var client = new HttpClient();
            var clientAddress = new Uri(subscriberUrl);
            client.BaseAddress = clientAddress;

            var dto = _mapper.Map<DTOs.WebhookMessage>(body);
            var json = JsonConvert.SerializeObject(dto);
            var data = new StringContent(json, Encoding.UTF8, "application/json");
            
            var response = await client.PostAsync($"{client.BaseAddress}?id={body.TrackingId}", data);
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                _logger.LogInformation("Notification was successful");
            }
            
            var result = response.Content.ReadAsStringAsync().Result;
            _logger.LogInformation("Client response: " + result);
        }
    }
}
