using System;
using System.Linq;
using Elsa.SKS.Backend.DataAccess.Entities;
using Elsa.SKS.Backend.DataAccess.Interfaces;
using Elsa.SKS.Backend.DataAccess.Sql;
using Elsa.SKS.Backend.DataAccess.Sql.Exceptions;
using FakeItEasy;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Elsa.SKS.Backend.DataAccess.Tests
{
    public class SubscriberRepositoryTest : DataAccessTests
    {
        private readonly IAppDbContext _context;

        private readonly ISubscriberRepository _subscriberRepository;

        private readonly ILogger<SubscriberRepository> _logger;

        public SubscriberRepositoryTest()
        {
            _context = GetMockedAppDbContext();
            _logger = A.Fake<ILogger<SubscriberRepository>>();
            _subscriberRepository = new SubscriberRepository(_context, _logger);
        }

        [Fact]
        public void GivenASubscription_WhenCreatingSubscription_ThenAddSubscriptionAndReturnIt()
        {
            var subscription = Builder<Subscription>
                .CreateNew()
                .Build();

            var createdSubscription = _subscriberRepository.Create(subscription);

            _context.Subscriptions.Count().Should().Be(1);
            _context.Subscriptions.Should().Contain(subscription);
            createdSubscription.Should().Be(subscription);
        }

        [Fact]
        public void GivenADbUpdateExceptionExceptionIsThrown_WhenCreatingSubscription_ThenADataAccessExceptionIsThrown()
        {
            var subscription = Builder<Subscription>
                .CreateNew()
                .Build();

            A.CallTo(() => _context.Subscriptions.Add(A<Subscription>._)).Throws<DbUpdateException>();
            Action create = () => _subscriberRepository.Create(subscription);

            create.Should().Throw<DataAccessException>();
        }

        [Fact]
        public void GivenADbUpdateConcurrencyExceptionIsThrown_WhenCreatingSubscription_ThenADataAccessExceptionIsThrown()
        {
            var subscription = Builder<Subscription>
                .CreateNew()
                .Build();

            A.CallTo(() => _context.Subscriptions.Add(A<Subscription>._)).Throws<DbUpdateConcurrencyException>();
            Action create = () => _subscriberRepository.Create(subscription);

            create.Should().Throw<DataAccessException>();
        }

        [Fact]
        public void GivenASubscription_WhenUpdatingSubscription_ThenUpdateAndReturnHop()
        {
            var subscription = Builder<Subscription>
                .CreateNew()
                .Build();

            _context.Subscriptions.Add(subscription);

            subscription.Url = "my_url";

            var updatedSubscription = _subscriberRepository.Update(subscription);

            _context.Subscriptions.Count().Should().Be(1);
            _context.Subscriptions.First().Url.Should().Be(subscription.Url);
            updatedSubscription.Should().Be(subscription);
        }

        [Fact]
        public void GivenADbUpdateExceptionExceptionIsThrown_WhenUpdatingSubscription_ThenADataAccessExceptionIsThrown()
        {
            var subscription = Builder<Subscription>
                .CreateNew()
                .Build();

            A.CallTo(() => _context.Subscriptions.Update(A<Subscription>._)).Throws<DbUpdateException>();
            Action create = () => _subscriberRepository.Update(subscription);

            create.Should().Throw<DataAccessException>();
        }

        [Fact]
        public void GivenADbUpdateConcurrencyExceptionIsThrown_WhenUpdatingSubscription_ThenADataAccessExceptionIsThrown()
        {
            var subscription = Builder<Subscription>
                .CreateNew()
                .Build();

            A.CallTo(() => _context.Subscriptions.Update(A<Subscription>._)).Throws<DbUpdateConcurrencyException>();
            Action create = () => _subscriberRepository.Update(subscription);

            create.Should().Throw<DataAccessException>();
        }

        [Fact]
        public void GivenASubscriptionExists_WhenDeletingSubscription_ThenReturnTrue()
        {
            var subscription = Builder<Subscription>
                .CreateNew()
                .Build();

            _context.Subscriptions.Add(subscription);

            bool subscriptionWasDeleted = _subscriberRepository.Delete(subscription.Id);

            _context.Subscriptions.Count().Should().Be(0);
            subscriptionWasDeleted.Should().BeTrue();
        }

        [Fact]
        public void GivenASubscriptionDoesNotExist_WhenDeletingSubscription_ThenReturnFalse()
        {
            var subscription = Builder<Subscription>
                .CreateNew()
                .Build();

            bool subscriptionWasDeleted = _subscriberRepository.Delete(subscription.Id);

            _context.Hops.Count().Should().Be(0);
            subscriptionWasDeleted.Should().BeFalse();
        }


        [Fact]
        public void GivenADbUpdateExceptionExceptionIsThrown_WhenDeletingSubscription_ThenADataAccessExceptionIsThrown()
        {
            var subscription = Builder<Subscription>
                .CreateNew()
                .Build();

            _context.Subscriptions.Add(subscription);
            A.CallTo(() => _context.Subscriptions.Remove(A<Subscription>._)).Throws<DbUpdateException>();
            Action delete = () => _subscriberRepository.Delete(subscription.Id);

            delete.Should().Throw<DataAccessException>();
        }

        [Fact]
        public void GivenADbUpdateConcurrencyExceptionIsThrown_WhenDeletingSubscription_ThenADataAccessExceptionIsThrown()
        {
            var subscription = Builder<Subscription>
                .CreateNew()
                .Build();

            _context.Subscriptions.Add(subscription);
            A.CallTo(() => _context.Subscriptions.Remove(A<Subscription>._)).Throws<DbUpdateConcurrencyException>();
            Action create = () => _subscriberRepository.Delete(subscription.Id);

            create.Should().Throw<DataAccessException>();
        }

        [Fact]
        public void GivenSubscriptionForTrackingIdExist_WhenDeleteAllByTrackingId_ThenReturnsTrue()
        {
            var subscription = Builder<Subscription>
                .CreateNew()
                .With(s => s.TrackingId = "my_tracking_id")
                .Build();

            _context.Subscriptions.Add(subscription);
            var isDeleted = _subscriberRepository.DeleteAllByTrackingId(subscription.TrackingId);

            isDeleted.Should().BeTrue();
        }

        [Fact]
        public void GivenSubscriptionForTrackingIdDoesNotExist_WhenDeleteAllByTrackingId_ThenReturnsTrue()
        {
            var subscription = Builder<Subscription>
                .CreateNew()
                .With(s => s.TrackingId = "my_tracking_id")
                .Build();

            _context.Subscriptions.Add(subscription);
            var subscriptionsWereDeleted = _subscriberRepository.DeleteAllByTrackingId(subscription.TrackingId);

            subscriptionsWereDeleted.Should().BeTrue();
        }

        [Fact]
        public void GivenSubscriptionsForTrackingIdExist_WhenGettingByTrackingId_ThenReturnsAllSubscriptions()
        {
            var subscription1 = Builder<Subscription>
                .CreateNew()
                .With(s => s.TrackingId = "my_tracking_id")
                .Build();

            var subscription2 = Builder<Subscription>
                .CreateNew()
                .With(s => s.TrackingId = "my_tracking_id")
                .Build();

            _context.Subscriptions.Add(subscription1);
            _context.Subscriptions.Add(subscription2);

            var requestedSubscriptions = _subscriberRepository.GetByTrackingId(subscription1.TrackingId);

            requestedSubscriptions.Count().Should().Be(2);
        }

        [Fact]
        public void GivenSubscriptionsForTrackingIdDoNotExist_WhenGettingByTrackingId_ThenReturnsEmptyEnumerable()
        {
            var subscription = Builder<Subscription>
                .CreateNew()
                .With(s => s.TrackingId = "my_tracking_id")
                .Build();

            var trackingIdOfNonExistentParcel = "my_non_existent_tracking_id";

            _context.Subscriptions.Add(subscription);
            var requestedSubscription = _subscriberRepository.GetByTrackingId(trackingIdOfNonExistentParcel);

            requestedSubscription.Count().Should().Be(0);
        }

        [Fact]
        public void GivenADbUpdateExceptionExceptionIsThrown_WhenGettingByTrackingId_ThenADataAccessExceptionIsThrown()
        {
            var subscription = Builder<Subscription>
                .CreateNew()
                .Build();

            A.CallTo(() => _context.Subscriptions).Throws<DbUpdateException>();
            Action getByTrackingId = () => _subscriberRepository.GetByTrackingId(subscription.TrackingId);

            getByTrackingId.Should().Throw<DataAccessException>();
        }

        [Fact]
        public void GivenSubscriptionForTrackingIdDoesExist_WhenGettingById_ThenReturnsSubscription()
        {
            var subscription = Builder<Subscription>
                .CreateNew()
                .With(s => s.Id = 1)
                .Build();

            _context.Subscriptions.Add(subscription);
            var requestedSubscription = _subscriberRepository.GetById(subscription.Id);

            requestedSubscription.Should().Be(subscription);
        }

        [Fact]
        public void GivenSubscriptionForTrackingIdDoesNotExist_WhenGettingById_ThenReturnsSubscription()
        {
            var subscription = Builder<Subscription>
                .CreateNew()
                .With(s => s.TrackingId = "my_tracking_id")
                .Build();

            var idOfNonExistentParcel = 0;

            _context.Subscriptions.Add(subscription);
            var requestedSubscription = _subscriberRepository.GetById(idOfNonExistentParcel);

            requestedSubscription.Should().BeNull();
        }

    }
}