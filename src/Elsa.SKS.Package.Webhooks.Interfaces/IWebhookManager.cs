using System.Collections.Generic;
using Elsa.SKS.Package.DataAccess.Entities;

namespace Elsa.SKS.Package.Webhooks.Interfaces
{
    public interface IWebhookManager
    {
        Subscription AddSubscription(Subscription subscriptionDal);
        List<Subscription> GetParcelWebhooks(string trackingId);
    }
}