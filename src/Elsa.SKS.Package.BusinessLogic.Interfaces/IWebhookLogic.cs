using System.Collections.Generic;
using Elsa.SKS.Package.BusinessLogic.Entities;

namespace Elsa.SKS.Package.BusinessLogic.Interfaces
{
    public interface IWebhookLogic
    {
        List<Subscription> GetParcelWebhooks(string trackingId);
    }
}