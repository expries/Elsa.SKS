using System.Collections.Generic;
using Elsa.SKS.Package.BusinessLogic.Entities;

namespace Elsa.SKS.Package.BusinessLogic.Interfaces
{
    public interface IWebhookLogic
    {
        Subscription SubscribeParcelWebhook(string trackingId, string url);
        List<Subscription> GetParcelWebhooks(string trackingId);
    }
}