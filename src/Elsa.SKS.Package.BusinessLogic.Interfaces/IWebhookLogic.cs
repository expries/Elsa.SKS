using System.Collections.Generic;
using Elsa.SKS.Package.BusinessLogic.Entities;

namespace Elsa.SKS.Package.BusinessLogic.Interfaces
{
    public interface IWebhookLogic
    {
        public Subscription SubscribeParcelWebhook(string trackingId, string url);
        public bool UnsubscribeParcelWebhook(long? id);
        public List<Subscription> GetParcelWebhooks(string trackingId);
    }
}