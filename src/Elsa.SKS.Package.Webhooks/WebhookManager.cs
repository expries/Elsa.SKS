using System;
using System.Collections.Generic;
using System.Linq;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.DataAccess.Entities;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
using Elsa.SKS.Package.Webhooks.Interfaces;
using Microsoft.Extensions.Logging;

namespace Elsa.SKS.Package.Webhooks
{
    public class WebhookManager : IWebhookManager
    {
        private readonly ISubscriberRepository _subscriberRepository;
        
        private readonly IParcelRepository _parcelRepository;

        private readonly ILogger<WebhookManager> _logger;

        public WebhookManager(ISubscriberRepository subscriberRepository, IParcelRepository parcelRepository, ILogger<WebhookManager> logger)
        {
            _subscriberRepository = subscriberRepository;
            _parcelRepository = parcelRepository;
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
        
    }
}
