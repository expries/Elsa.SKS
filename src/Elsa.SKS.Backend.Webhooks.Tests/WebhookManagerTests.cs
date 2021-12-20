using System;
using System.Collections.Generic;
using AutoMapper;
using Elsa.SKS.Backend.BusinessLogic.Exceptions;
using Elsa.SKS.Backend.DataAccess.Entities;
using Elsa.SKS.Backend.DataAccess.Interfaces;
using Elsa.SKS.Backend.DataAccess.Sql.Exceptions;
using Elsa.SKS.Backend.Webhooks.Interfaces;
using FakeItEasy;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Elsa.SKS.Backend.Webhooks.Tests
{
    public class WebhookManagerTests
    {
        private readonly IWebhookManager _webhookManager;

        private readonly ISubscriberRepository _subscriberRepository;
        
        private readonly IParcelRepository _parcelRepository;

        private readonly IMapper _mapper;
        
        private readonly ILogger<WebhookManager> _logger;
        
        public WebhookManagerTests()
        {
            _webhookManager = A.Fake<IWebhookManager>();
            _subscriberRepository = A.Fake<ISubscriberRepository>();
            _parcelRepository = A.Fake<IParcelRepository>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<WebhookManager>>();
            _webhookManager = new WebhookManager(_subscriberRepository, _parcelRepository, _mapper, _logger);
        }
        
        [Fact]
        public void GivenANewSubscription_WhenAddingSubscription_ThenReturnSubscription()
        {
            const string trackingId = "my_tracking_id";
            
            var newSubscription = Builder<Subscription>
                .CreateNew()
                .With(s => s.TrackingId == trackingId)
                .Build();

            var parcel = Builder<Parcel>
                .CreateNew()
                .Build();
            
            A.CallTo(() => _parcelRepository.GetByTrackingId(newSubscription.TrackingId)).Returns(parcel);
            A.CallTo(() => _subscriberRepository.Create(newSubscription)).Returns(newSubscription);

            var result = _webhookManager.AddSubscription(newSubscription);
            result.Should().Be(newSubscription);
        }
        
        [Fact]
        public void GivenParcelThatDoesNotExist_WhenAddingSubscription_ThenReturnParcelNotFoundException()
        {
            const string trackingId = "my_tracking_id";
            
            var newSubscription = Builder<Subscription>
                .CreateNew()
                .With(s => s.TrackingId == trackingId)
                .Build();

            var parcel = Builder<Parcel>
                .CreateNew()
                .Build();
            
            A.CallTo(() => _parcelRepository.GetByTrackingId(newSubscription.TrackingId)).Returns(null);
            A.CallTo(() => _subscriberRepository.Create(newSubscription)).Returns(newSubscription);

            Action create = () =>  _webhookManager.AddSubscription(newSubscription);
            create.Should().Throw<ParcelNotFoundException>();
        }
        
        [Fact]
        public void GivenADbAccessExceptionIsThrown_WhenAddingSubscription_ThenReturnDbAccessException()
        {
            const string trackingId = "my_tracking_id";
            
            var newSubscription = Builder<Subscription>
                .CreateNew()
                .With(s => s.TrackingId == trackingId)
                .Build();

            var parcel = Builder<Parcel>
                .CreateNew()
                .Build();
            
            A.CallTo(() => _parcelRepository.GetByTrackingId(newSubscription.TrackingId)).Throws<DataAccessException>();
            A.CallTo(() => _subscriberRepository.Create(newSubscription)).Returns(newSubscription);

            Action create = () =>  _webhookManager.AddSubscription(newSubscription);
            create.Should().Throw<DataAccessException>();
        }
        
        [Fact]
        public void GivenADbAccessExceptionIsThrown_WhenDeleteAllSubscriptionsByTrackingId_ThenReturnDbAccessException()
        {
            const string trackingId = "my_tracking_id";
            
            var newSubscription = Builder<Subscription>
                .CreateNew()
                .With(s => s.TrackingId == trackingId)
                .Build();

            A.CallTo(() => _subscriberRepository.DeleteAllByTrackingId(trackingId)).Throws<DataAccessException>();

            Action create = () => _webhookManager.DeleteAllSubscriptionsByTrackingId(trackingId);
            create.Should().Throw<DataAccessException>();
        }
        
    }
}
