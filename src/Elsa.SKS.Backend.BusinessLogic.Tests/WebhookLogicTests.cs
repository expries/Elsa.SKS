using System;
using AutoMapper;
using Elsa.SKS.Backend.BusinessLogic.Entities;
using Elsa.SKS.Backend.BusinessLogic.Exceptions;
using Elsa.SKS.Backend.BusinessLogic.Interfaces;
using Elsa.SKS.Backend.DataAccess.Sql.Exceptions;
using Elsa.SKS.Backend.Webhooks.Interfaces;
using FakeItEasy;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Elsa.SKS.Backend.BusinessLogic.Tests
{
    public class WebhookLogicTests
    {
        private readonly IWebhookLogic _logic;

        private readonly IWebhookManager _webhookManager;

        private readonly IMapper _mapper;
        
        private readonly ILogger<WebhookLogic> _logger;

        public WebhookLogicTests()
        {
            _webhookManager = A.Fake<IWebhookManager>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<WebhookLogic>>();
            _logic = new WebhookLogic(_webhookManager, _mapper, _logger);
        }

        [Fact]
        public void GivenATrackingIdAndUrl_WhenSubscribingParcelWebhook_ThenReturnSubscription()
        {
            const string trackingId = "my_tracking_id";
            const string url = "my_url";

            var subscription = Builder<Subscription>
                .CreateNew()
                .Build();
            
            var subscriptionEntity = Builder<DataAccess.Entities.Subscription>
                .CreateNew()
                .Build();

            A.CallTo(() => _mapper.Map<Subscription>(A<DataAccess.Entities.Subscription>._)).Returns(subscription);

            var result = _logic.SubscribeParcelWebhook(trackingId, url);
            result.Should().Be(subscription);
        }
        
        [Fact]
        public void GivenADbAccessExceptionIsThrown_WhenSubscribingParcelWebhook_ThenABusinessExceptionIsThrown()
        {
            const string trackingId = "my_tracking_id";
            const string url = "my_url";
            
            A.CallTo(() => _webhookManager.AddSubscription(A<DataAccess.Entities.Subscription>._)).Throws<DataAccessException>();

            Action create = () => _logic.SubscribeParcelWebhook(trackingId, url);
            
            create.Should().Throw<BusinessException>();
        }
        
        [Fact]
        public void GivenADbAccessExceptionIsThrown_WhenUnsubscribingParcelWebhook_ThenABusinessExceptionIsThrown()
        {
            const long id = 1;
            
            A.CallTo(() => _webhookManager.DeleteSubscriptionById(id)).Throws<DataAccessException>();

            Action create = () => _logic.UnsubscribeParcelWebhook(id);
            
            create.Should().Throw<BusinessException>();
        }
        
        [Fact]
        public void GivenADbAccessExceptionIsThrown_WhenGettingParcelWebhooks_ThenABusinessExceptionIsThrown()
        {
            const long id = 1;
            
            A.CallTo(() => _webhookManager.DeleteSubscriptionById(id)).Throws<ParcelNotFoundException>();

            Action create = () => _logic.UnsubscribeParcelWebhook(id);
            
            create.Should().Throw<ParcelNotFoundException>();
        }
    }
}