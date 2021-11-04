using System;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.DataAccess.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Xunit;

namespace Elsa.SKS.Package.BusinessLogic.Tests
{
    public class ParcelTrackingLogicTests
    {
        private readonly IParcelTrackingLogic _logic;

        private readonly IParcelRepository _parcelRepository;

        private readonly IHopRepository _hopRepository;

        private readonly IMapper _mapper;
        
        public ParcelTrackingLogicTests()
        {
            _parcelRepository = A.Fake<IParcelRepository>();
            _hopRepository = A.Fake<IHopRepository>();
            _mapper = A.Fake<IMapper>();
            _logic = new ParcelTrackingLogic(_parcelRepository, _hopRepository, _mapper);
        }
        
        [Fact]
        public void GivenCorrectTrackingId_WhenReportingParcelDelivery_ThenReturnParcelWithCorrectTrackingId()
        {
            const string trackingId = TestConstants.TrackingIdOfParcelThatIsTransferred;
            
            Action reportParcelDelivery = () => _logic.ReportParcelDelivery(trackingId);

            reportParcelDelivery.Should().NotThrow<Exception>();
        }
        
        [Fact]
        public void GivenTrackingIdOfNonExistentParcel_WhenReportingParcelDelivery_ThenThrowParcelNotFoundException()
        {
            const string trackingId = TestConstants.TrackingIdOfNonExistentParcel;
            
            Assert.Throws<ParcelNotFoundException>(() => _logic.ReportParcelDelivery(trackingId));

        }
        
        [Fact]
        public void GivenTrackingIdOfParcelThatCanNotBeReported_WhenReportingParcelDelivery_ThenThrowReportParcelHopException()
        {
            const string trackingId = TestConstants.TrackingIdOfParcelThatCanNotBeReported;
            
            Assert.Throws<ReportParcelHopException>(() => _logic.ReportParcelDelivery(trackingId));
        }
        
        [Fact]
        public void GivenTrackingIdOfNonExistentParcel_WhenReportingParcelHop_ThenThrowParcelNotFoundException()
        {
            const string code = TestConstants.ExistentHopCode;
            const string trackingId = TestConstants.TrackingIdOfNonExistentParcel;
            
            Assert.Throws<ParcelNotFoundException>(() => _logic.ReportParcelHop(trackingId, code));
        }
        
        [Fact]
        public void GivenNonExistentHopCode_WhenReportingParcelHop_ThenThrowHopNotFoundException()
        {
            const string code = TestConstants.NonExistentHopCode;
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;
            
            Assert.Throws<HopNotFoundException>(() => _logic.ReportParcelHop(trackingId, code));
        }
        
        [Fact]
        public void GivenTrackingIdOfParcelThatCanNotBeReported_WhenReportingParcelHop_ThenThrowReportParcelHopException()
        {
            const string code = TestConstants.ExistentHopCode;
            const string trackingId = TestConstants.TrackingIdOfParcelThatCanNotBeReported;
            
            Assert.Throws<ReportParcelHopException>(() => _logic.ReportParcelHop(trackingId, code));
        }
        
        [Fact]
        public void GivenCorrectTrackingId_WhenTrackingParcel_ThenReturnParcelWithCorrectTrackingId()
        {
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;
            
            var parcelReturned = _logic.TrackParcel(trackingId);
            
            parcelReturned.Should().BeOfType<Parcel>();
            parcelReturned.TrackingId.Should().Be($"{TestConstants.TrackingIdOfExistentParcel}");
        }
        
        [Fact]
        public void GivenTrackingIdOfNonExistentParcel_WhenTrackingParcel_ThenThrowParcelNotFoundException()
        {
            const string trackingId = TestConstants.TrackingIdOfNonExistentParcel;
            
            Assert.Throws<ParcelNotFoundException>(() => _logic.TrackParcel(trackingId));
        }
        
        [Fact]
        public void GivenTrackingIdOfParcelThatCanNotBeTracked_WhenTrackingParcel_ThenThrowTrackingException()
        {
            const string trackingId = TestConstants.TrackingIdOfParcelThatCanNotBeTracked;
            
            Assert.Throws<TrackingException>(() => _logic.TrackParcel(trackingId));
        }
    }
}