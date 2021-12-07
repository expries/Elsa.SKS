using System;
using System.Collections.Generic;
using System.Linq;
using Elsa.SKS.Package.DataAccess.Entities;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.WebHooks.Interfaces;

namespace Elsa.SKS.Package.WebHooks
{
    public class WebhookManager : IWebhookManager
    {
        private readonly ISubscriberRepository _subscriberRepository;

        public WebhookManager(ISubscriberRepository subscriberRepository)
        {
            _subscriberRepository = subscriberRepository;
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
