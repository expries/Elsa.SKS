using System;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
using FakeItEasy;
using FizzWare.NBuilder;
using FluentAssertions;
using Xunit;
using Hop = Elsa.SKS.Package.DataAccess.Entities.Hop;

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
        public void GivenAParcelExists_WhenReportingParcelDelivery_ThenReturnSuccessfully()
        {
            const string trackingId = "my_trackingId";
            
            Action reportParcelDelivery = () => _logic.ReportParcelDelivery(trackingId);

            reportParcelDelivery.Should().NotThrow<Exception>();
        }
        
        [Fact]
        public void GivenAParcelDoesNotExist_WhenReportingParcelDelivery_ThenThrowParcelNotFoundException()
        {
            const string trackingId = "my_trackingId";

            A.CallTo(() => _parcelRepository.GetByTrackingId(A<string>._)).Returns(null);

            Action reportParcelDelivery = () => _logic.ReportParcelDelivery(trackingId);

            reportParcelDelivery.Should().Throw<ParcelNotFoundException>();

        }
        
        [Fact]
        public void GivenADataAccessErrorOccurs_WhenReportingParcelDelivery_ThenThrowABusinessException()
        {
            const string trackingId = "my_trackingId";

            A.CallTo(() => _parcelRepository.GetByTrackingId(A<string>._)).Throws<DataAccessException>();

            Action reportParcelDelivery = () => _logic.ReportParcelDelivery(trackingId);

            reportParcelDelivery.Should().Throw<BusinessException>();
        }
        
        [Fact]
        public void GivenATrackingIdAndHopCode_WhenReportingParcelHop_ThenReturnSuccessfully()
        {
           const string trackingId = "my_trackingId";
           const string hopCode = "hop_code";

           var parcel = Builder<DataAccess.Entities.Parcel>
                .CreateNew()
                .Build();

            var hop = Builder<Hop>
                .CreateNew()
                .Build();
            
            A.CallTo(() => _parcelRepository.GetByTrackingId(A<string>._)).Returns(parcel);
            A.CallTo(() => _hopRepository.GetByCode(A<string>._)).Returns(hop);

            Action reportParcelHop = () => _logic.ReportParcelHop(trackingId, hopCode);

            reportParcelHop.Should().NotThrow<Exception>();
        }
        
        [Fact]
        public void GivenAParcelDoesNotExist_WhenReportingParcelHop_ThenThrowAParcelNotFoundException()
        {
            const string trackingId = "my_trackingId";
            const string hopCode = "hop_code";

            A.CallTo(() => _parcelRepository.GetByTrackingId(A<string>._)).Returns(null);

            Action reportParcelHop = () => _logic.ReportParcelHop(trackingId, hopCode);

            reportParcelHop.Should().Throw<ParcelNotFoundException>();
        }
        
        [Fact]
        public void GivenAHopDoesNotExist_WhenReportingParcelHop_ThenThrowAHopNotFoundException()
        {
            const string trackingId = "my_trackingId";
            const string hopCode = "hop_code";

            var parcel = Builder<DataAccess.Entities.Parcel>
                .CreateNew()
                .Build();

            A.CallTo(() => _parcelRepository.GetByTrackingId(A<string>._)).Returns(parcel);
            A.CallTo(() => _hopRepository.GetByCode(A<string>._)).Returns(null);

            Action reportParcelHop = () => _logic.ReportParcelHop(trackingId, hopCode);

            reportParcelHop.Should().Throw<HopNotFoundException>();
        }
        
        [Fact]
        public void GivenADataAccessErrorOccurs_WhenReportingParcelHop_ThenThrowABusinessException()
        {
            const string trackingId = "my_trackingId";
            const string hopCode = "hop_code";

            A.CallTo(() => _parcelRepository.GetByTrackingId(A<string>._)).Throws<DataAccessException>();
            
            Action reportParcelHop = () => _logic.ReportParcelHop(trackingId, hopCode);

            reportParcelHop.Should().Throw<BusinessException>();
        }
        
        [Fact]
        public void GivenAParcelExists_WhenTrackingParcel_ThenReturnTheParcel()
        {
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;
            
            var parcel = Builder<Parcel>
                .CreateNew()
                .Build();
            
            var parcelEntity = Builder<DataAccess.Entities.Parcel>
                .CreateNew()
                .Build();

            A.CallTo(() => _parcelRepository.GetByTrackingId(A<string>._)).Returns(parcelEntity);
            A.CallTo(() => _mapper.Map<Parcel>(A<DataAccess.Entities.Parcel>._)).Returns(parcel);
            
            var parcelReturned = _logic.TrackParcel(trackingId);

            parcelReturned.Should().Be(parcel);
        }
        
        [Fact]
        public void GivenAParcelDoesNotExist_WhenTrackingParcel_ThenThrowAParcelNotFoundException()
        {
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;
            
            A.CallTo(() => _parcelRepository.GetByTrackingId(A<string>._)).Throws<ParcelNotFoundException>();

            Action trackParcel = () => _logic.TrackParcel(trackingId);

            trackParcel.Should().Throw<ParcelNotFoundException>();
        }
        
        [Fact]
        public void GivenADataAccessErrorOccurs_WhenTrackingParcel_ThenThrowABusinessException()
        {
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;

            A.CallTo(() => _parcelRepository.GetByTrackingId(A<string>._)).Throws<DataAccessException>();

            Action trackParcel = () => _logic.TrackParcel(trackingId);

            trackParcel.Should().Throw<BusinessException>();
        }
    }
}