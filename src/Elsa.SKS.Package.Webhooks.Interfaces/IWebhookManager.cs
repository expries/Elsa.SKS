using System.Collections.Generic;
using Elsa.SKS.Package.DataAccess.Entities;

namespace Elsa.SKS.Package.Webhooks.Interfaces
{
    public interface IWebhookManager
    {
        public Subscription AddSubscription(Subscription subscriptionDal);
        public List<Subscription> GetParcelWebhooks(string trackingId);
        public bool DeleteSubscriptionById(long? id);
    }
}