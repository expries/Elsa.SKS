using System;
using System.Collections.Generic;
using System.Transactions;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.BusinessLogic.Validators;
using FakeItEasy;
using FizzWare.NBuilder;
using FluentAssertions;
using Xunit;

namespace Elsa.SKS.Package.BusinessLogic.Tests
{
    public class ParcelRegistrationTests
    {

        [Fact]
        public void GivenTheParcelInformationIsCorrect_WhenTransitioningTheParcel_ThenReturnParcel()
        {
            var parcelRegistration = new ParcelRegistration();
            var parcel = Builder<Parcel>.CreateNew()
                .With(x => x.Recipient = new User())
                .And(x => x.Sender = new User())
                .And(x => x.VisitedHops = new List<HopArrival>())
                .And(x => x.FutureHops = new List<HopArrival>())
                .Build();
            const string trackingId = TestConstants.TrackingIdOfParcelThatIsTransferred;
            
            var parcelReturned = parcelRegistration.TransitionParcel(parcel, trackingId);
            
            parcelReturned.Should().BeOfType<Parcel>();
        }
        
        [Fact]
        public void GivenTheValidationIsNotValid_WhenTransitioningTheParcel_ThenThrowTransactionException()
        {
            var parcelRegistration = new ParcelRegistration();
            var parcel = Builder<Parcel>.CreateNew()
                .Build();
            const string trackingId = TestConstants.TrackingIdOfParcelThatIsTransferred;
            
            Assert.Throws<TransactionException>(() => parcelRegistration.TransitionParcel(parcel, trackingId));
        }
        
        [Fact]
        public void GivenTheTrackingIdIsWrong_WhenTransitioningTheParcel_ThenThrowTransferException()
        {
            var parcelRegistration = new ParcelRegistration();
            var parcel = Builder<Parcel>.CreateNew()
                .With(x => x.Recipient = new User())
                .And(x => x.Sender = new User())
                .And(x => x.VisitedHops = new List<HopArrival>())
                .And(x => x.FutureHops = new List<HopArrival>())
                .Build();
            const string trackingId = TestConstants.TrackingIdOfParcelThatIsNotTransferred;
            
            Assert.Throws<TransferException>(() => parcelRegistration.TransitionParcel(parcel, trackingId));
        }
        
        [Fact]
        public void GivenTheParcelInformationIsCorrect_WhenSubmittingTheParcel_ThenReturnParcelWithCorrectTrackingId()
        {
            var parcelRegistration = new ParcelRegistration();
            var parcel = Builder<Parcel>.CreateNew()
                .With(x => x.Recipient = new User())
                .And(x => x.Sender = new User())
                .And(x => x.VisitedHops = new List<HopArrival>())
                .And(x => x.FutureHops = new List<HopArrival>())
                .Build();
            
            var parcelReturned = parcelRegistration.SubmitParcel(parcel);

            parcelReturned.Should().BeOfType<Parcel>();
            parcelReturned.TrackingId.Should().Be($"{TestConstants.TrackingIdOfSubmittedParcel}");
        }
        
        [Fact]
        public void GivenTheValidationIsNotValid_WhenSubmittingTheParcel_ThenReturnInvalidParcelException()
        {
            var parcelRegistration = new ParcelRegistration();
            var parcel = Builder<Parcel>.CreateNew().Build();
            
            Assert.Throws<InvalidParcelException>(() => parcelRegistration.SubmitParcel(parcel));
        }
        
        [Fact]
        public void GivenTheWeightIsTooLow_WhenSubmittingTheParcel_ThenReturnInvalidParcelException()
        {
            var parcelRegistration = new ParcelRegistration();
            var parcel = Builder<Parcel>.CreateNew()
                .With(x => x.Weight = -1)
                .Build();
            
            Assert.Throws<InvalidParcelException>(() => parcelRegistration.SubmitParcel(parcel));
        }
        


    }
}