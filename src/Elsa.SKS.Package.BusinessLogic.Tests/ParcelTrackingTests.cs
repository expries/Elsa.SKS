using System.Collections.Generic;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using FizzWare.NBuilder;
using FluentAssertions;
using Xunit;

namespace Elsa.SKS.Package.BusinessLogic.Tests
{
    public class ParcelTrackingTests
    {
        [Fact]
        public void GivenTheTrackingIdIsCorrect_WhenReportingParcelDelivery_ThenReturnParcelWithCorrectTrackingId()
        {
            var parcelTracking = new ParcelTracking();
            const string trackingId = TestConstants.TrackingIdOfParcelThatIsTransferred;
            
            var parcelReturned = parcelTracking.ReportParcelDelivery(trackingId);
            
            parcelReturned.Should().BeOfType<Parcel>();
            parcelReturned.TrackingId.Should().Be($"{TestConstants.TrackingIdOfParcelThatIsTransferred}");
        }
        
        [Fact]
        public void GivenTheTrackingIdIsNonExistent_WhenReportingParcelDelivery_ThenThrowParcelNotFoundException()
        {
            var parcelTracking = new ParcelTracking();
            const string trackingId = TestConstants.TrackingIdOfNonExistentParcel;
            
            Assert.Throws<ParcelNotFoundException>(() => parcelTracking.ReportParcelDelivery(trackingId));

        }
        
        [Fact]
        public void GivenTheTrackingIdFromParcelThatCanNotBeReported_WhenReportingParcelDelivery_ThenThrowReportParcelHopException()
        {
            var parcelTracking = new ParcelTracking();
            const string trackingId = TestConstants.TrackingIdOfParcelThatCanNotBeReported;
            
            Assert.Throws<ReportParcelHopException>(() => parcelTracking.ReportParcelDelivery(trackingId));
        }
        
        [Fact]
        public void GivenTheTrackingIdIsNonExistent_WhenReportingParcelHop_ThenThrowParcelNotFoundException()
        {
            var parcelTracking = new ParcelTracking();
            const string code = TestConstants.ExistentHopCode;
            const string trackingId = TestConstants.TrackingIdOfNonExistentParcel;
            
            Assert.Throws<ParcelNotFoundException>(() => parcelTracking.ReportParcelHop(trackingId, code));
        }
        
        [Fact]
        public void GivenTheHopCodeIsNonExistent_WhenReportingParcelHop_ThenThrowHopNotFoundException()
        {
            var parcelTracking = new ParcelTracking();
            const string code = TestConstants.NonExistentHopCode;
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;
            
            Assert.Throws<HopNotFoundException>(() => parcelTracking.ReportParcelHop(trackingId, code));
        }
        
        [Fact]
        public void GivenTheTrackingIdFromParcelThatCanNotBeReported_WhenReportingParcelHop_ThenThrowReportParcelHopException()
        {
            var parcelTracking = new ParcelTracking();
            const string code = TestConstants.ExistentHopCode;
            const string trackingId = TestConstants.TrackingIdOfParcelThatCanNotBeReported;
            
            Assert.Throws<ReportParcelHopException>(() => parcelTracking.ReportParcelHop(trackingId, code));
        }
        
        [Fact]
        public void GivenTheTrackingIdIsCorrect_WhenTrackingParcel_ThenReturnParcelWithCorrectTrackingId()
        {
            var parcelTracking = new ParcelTracking();
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;
            
            var parcelReturned = parcelTracking.TrackParcel(trackingId);
            
            parcelReturned.Should().BeOfType<Parcel>();
            parcelReturned.TrackingId.Should().Be($"{TestConstants.TrackingIdOfExistentParcel}");
        }
        
        [Fact]
        public void GivenTheTrackingIdIsNonExistent_WhenTrackingParcel_ThenThrowParcelNotFoundException()
        {
            var parcelTracking = new ParcelTracking();
            const string trackingId = TestConstants.TrackingIdOfNonExistentParcel;
            
            Assert.Throws<ParcelNotFoundException>(() => parcelTracking.TrackParcel(trackingId));
        }
        
        [Fact]
        public void GivenTheTrackingIdFromParcelThatCanNotBeTracked_WhenTrackingParcel_ThenThrowTrackingException()
        {
            var parcelTracking = new ParcelTracking();
            const string trackingId = TestConstants.TrackingIdOfParcelThatCanNotBeTracked;
            
            Assert.Throws<TrackingException>(() => parcelTracking.TrackParcel(trackingId));
        }
    }
}