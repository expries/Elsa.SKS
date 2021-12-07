using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.WebHooks.Interfaces;

namespace Elsa.SKS.Package.BusinessLogic
{
    public class WebhookLogic : IWebhookLogic
    {
        private readonly IWebhookManager _webhookManager;
        
        private readonly IMapper _mapper;


        public WebhookLogic(IWebhookManager webhookManager, IMapper mapper)
        {
            _webhookManager = webhookManager;
            _mapper = mapper;
        }

        public List<Subscription> GetParcelWebhooks(string trackingId)
        {
            var subscriptions = _webhookManager.GetParcelWebhooks(trackingId);
            // mapping
            var subBL = new List<Subscription>();
            return subBL;
        }
    }
}