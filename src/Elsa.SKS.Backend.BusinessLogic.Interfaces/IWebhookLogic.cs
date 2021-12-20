using System.Collections.Generic;
using Elsa.SKS.Backend.BusinessLogic.Entities;
using Elsa.SKS.Backend.BusinessLogic.Entities.Events;

namespace Elsa.SKS.Backend.BusinessLogic.Interfaces
{
    public interface IWebhookLogic
    {
        public Subscription SubscribeParcelWebhook(string trackingId, string url);
        public bool UnsubscribeParcelWebhook(long? id);
        public List<Subscription> GetParcelWebhooks(string trackingId);
        public void OnParcelDelivered(object source, ParcelEventArgs args);
        public void OnParcelStatusChanged(object sender, ParcelEventArgs args);
    }
}