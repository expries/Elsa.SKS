using System;
using System.Collections.Generic;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Entities.Events;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
using Elsa.SKS.Package.Webhooks.Interfaces;
using Microsoft.Extensions.Logging;

namespace Elsa.SKS.Package.BusinessLogic
{
    public class WebhookLogic : IWebhookLogic
    {
        private readonly IWebhookManager _webhookManager;
        
        private readonly IMapper _mapper;
        
        private readonly ILogger<WebhookLogic> _logger;

        public WebhookLogic(IWebhookManager webhookManager, IMapper mapper, ILogger<WebhookLogic> logger)
        {
            _webhookManager = webhookManager;
            _logger = logger;
            _mapper = mapper;
        }
        public Subscription SubscribeParcelWebhook(string trackingId, string url)
        {
            try
            {
                var newSubscription = new Subscription
                {
                    TrackingId = trackingId,
                    Url = url,
                    CreatedAt = DateTime.Now
                };

                var subscriptionDal = _mapper.Map<Elsa.SKS.Package.DataAccess.Entities.Subscription>(newSubscription);
                var subscriptionEntity = _webhookManager.AddSubscription(subscriptionDal);
                subscriptionDal = _mapper.Map<Elsa.SKS.Package.DataAccess.Entities.Subscription>(subscriptionEntity);
                
                // send confirmation to client
                _webhookManager.ConfirmRegistration(subscriptionDal);
                
                var result = _mapper.Map<Subscription>(subscriptionEntity);
                return result;
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Database error");
                throw new BusinessException("A database error has occurred.", ex);
            }
        }

        public bool UnsubscribeParcelWebhook(long? id)
        {
            try
            {
                var isUnsubscribed = _webhookManager.DeleteSubscriptionById(id);
                return isUnsubscribed;
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Database error");
                throw new BusinessException("A database error has occurred.", ex);
            }
        }

        public List<Subscription> GetParcelWebhooks(string trackingId)
        {
            var subscriptions = _webhookManager.GetParcelWebhooks(trackingId);
            var result = _mapper.Map<List<Subscription>>(subscriptions);
            return result;
        }
        
                
        public void OnParcelDelivered(object source, ParcelEventArgs args)
        {
            _logger.LogInformation($"Webhook received Parcel delivered event of parcel {args.Parcel.TrackingId}");
            Console.WriteLine();
            _webhookManager.DeleteAllSubscriptionsByTrackingId(args.Parcel.TrackingId);
        }

        public void OnParcelStatusChanged(object sender, ParcelEventArgs args)
        {
            _logger.LogInformation($"Webhook received Parcel status changed event of parcel {args.Parcel.TrackingId}");
            // map parcel to webhook message
            var webhookMessage = _mapper.Map<Package.DataAccess.Entities.WebhookMessage>(args.Parcel);
            _webhookManager.NotifySubscribers(webhookMessage);
        }
    }
}