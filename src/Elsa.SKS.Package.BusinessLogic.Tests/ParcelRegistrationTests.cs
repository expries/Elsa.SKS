using System.Collections.Generic;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Validators;
using FizzWare.NBuilder;
using FluentAssertions;
using Xunit;

namespace Elsa.SKS.Package.BusinessLogic.Tests
{
    public class ParcelRegistrationTests
    {
        [Fact]
        public void GivenCorrectParcelInformation_WhenTransitioningTheParcel_ThenReturnParcel()
        {
            var parcelRegistration = new ParcelRegistration(new ParcelValidator());
            var parcel = Builder<Parcel>.CreateNew()
                .With(x => x.Recipient = new User())
                .And(x => x.Sender = new User())
                .And(x => x.VisitedHops = new List<HopArrival>())
                .And(x => x.FutureHops = new List<HopArrival>())
                .With(x => x.TrackingId = TestConstants.ValidTrackingId)
                .Build();
            
            const string trackingId = TestConstants.TrackingIdOfParcelThatIsTransferred;
            
            var parcelReturned = parcelRegistration.TransitionParcel(parcel, trackingId);
            
            parcelReturned.Should().BeOfType<Parcel>();
        }
        
        [Fact]
        public void GivenNonValidParcelValidation_WhenTransitioningTheParcel_ThenThrowTransactionException()
        {
            var parcelRegistration = new ParcelRegistration(new ParcelValidator());
            var parcel = Builder<Parcel>.CreateNew().Build();
            const string trackingId = TestConstants.TrackingIdOfParcelThatIsTransferred;
            
            Assert.Throws<TransferException>(() => parcelRegistration.TransitionParcel(parcel, trackingId));
        }
        
        [Fact]
        public void GivenWrongTrackingId_WhenTransitioningTheParcel_ThenThrowTransferException()
        {
            var parcelRegistration = new ParcelRegistration(new ParcelValidator());
            var parcel = Builder<Parcel>.CreateNew()
                .With(x => x.Recipient = new User())
                .And(x => x.Sender = new User())
                .And(x => x.VisitedHops = new List<HopArrival>())
                .And(x => x.FutureHops = new List<HopArrival>())
                .With(x => x.TrackingId = TestConstants.ValidTrackingId)
                .Build();
            
            const string trackingId = TestConstants.TrackingIdOfParcelThatIsNotTransferred;
            
            Assert.Throws<TransferException>(() => parcelRegistration.TransitionParcel(parcel, trackingId));
        }
        
        [Fact]
        public void GivenCorrectParcelInformation_WhenSubmittingTheParcel_ThenReturnParcelWithCorrectTrackingId()
        {
            var parcelRegistration = new ParcelRegistration(new ParcelValidator());
            var parcel = Builder<Parcel>.CreateNew()
                .With(x => x.Recipient = new User())
                .And(x => x.Sender = new User())
                .And(x => x.VisitedHops = new List<HopArrival>())
                .And(x => x.FutureHops = new List<HopArrival>())
                .With(x => x.TrackingId = TestConstants.ValidTrackingId)
                .Build();
            
            var parcelReturned = parcelRegistration.SubmitParcel(parcel);

            parcelReturned.Should().BeOfType<Parcel>();
            parcelReturned.TrackingId.Should().Be($"{TestConstants.TrackingIdOfSubmittedParcel}");
        }
        
        [Fact]
        public void GivenNonValidParcelValidation_WhenSubmittingTheParcel_ThenReturnInvalidParcelException()
        {
            var parcelRegistration = new ParcelRegistration(new ParcelValidator());
            var parcel = Builder<Parcel>.CreateNew().Build();
            
            Assert.Throws<InvalidParcelException>(() => parcelRegistration.SubmitParcel(parcel));
        }
        
        [Fact]
        public void GivenTooLowParcelWeight_WhenSubmittingTheParcel_ThenReturnInvalidParcelException()
        {
            var parcelRegistration = new ParcelRegistration(new ParcelValidator());
            var parcel = Builder<Parcel>.CreateNew()
                .With(x => x.Weight = -1)
                .Build();
            
            Assert.Throws<InvalidParcelException>(() => parcelRegistration.SubmitParcel(parcel));
        }
    }
}