using System.Collections.Generic;
using Elsa.SKS.Package.DataAccess.Entities;

namespace Elsa.SKS.Package.WebHooks.Interfaces
{
    public interface IWebhookManager
    {
        List<Subscription> GetParcelWebhooks(string trackingId);
    }
}