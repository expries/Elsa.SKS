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
        
        private readonly ILogger<WebhookManager> _logger;

        public WebhookManager(ISubscriberRepository subscriberRepository, ILogger<WebhookManager> logger)
        {
            _subscriberRepository = subscriberRepository;
            _logger = logger;
        }

        public Subscription AddSubscription(Subscription newSubscription)
        {
            try
            {
                var subscription = _subscriberRepository.Create(newSubscription);
                return subscription;
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Database error");
                throw new DataAccessException("A database error has occurred.", ex);
            }
        }

        public List<Subscription> GetParcelWebhooks(string trackingId)
        {
            var result = new List<Subscription>();
            
            try
            {
                result = _subscriberRepository.GetByTrackingId(trackingId).ToList();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
            
            //
            return result;
        }
    }
}
