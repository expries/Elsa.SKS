using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Elsa.SKS.Backend.DataAccess.Entities;

namespace Elsa.SKS.Backend.Webhooks.Interfaces
{
    public interface IWebhookManager
    {
        public Subscription AddSubscription(Subscription newSubscription);
        public bool DeleteSubscriptionById(long? id);
        public bool DeleteAllSubscriptionsByTrackingId(string trackingId);
        public List<Subscription> GetParcelWebhooks(string trackingId);
        public Task ConfirmRegistration(Subscription subscription);
        public Task NotifySubscribers(WebhookMessage parcel);
    }
}