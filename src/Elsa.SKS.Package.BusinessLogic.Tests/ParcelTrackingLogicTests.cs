using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
using FakeItEasy;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.Extensions.Logging;
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
        
        private readonly ILogger<ParcelTrackingLogic> _logger;

        public ParcelTrackingLogicTests()
        {
            _parcelRepository = A.Fake<IParcelRepository>();
            _hopRepository = A.Fake<IHopRepository>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<ParcelTrackingLogic>>();
            _logic = new ParcelTrackingLogic(_parcelRepository, _hopRepository, _mapper, _logger);
        }
        
        [Fact]
        public void GivenAParcelExists_WhenReportingParcelDelivery_ThenReturnSuccessfully()
        {
            const string trackingId = "my_trackingId";

            var parcel = Builder<Parcel>
                .CreateNew()
                .With(p => p.FutureHops = new List<HopArrival>
                {
                    new HopArrival()
                })
                .Build();

            A.CallTo(() => _mapper.Map<Parcel>(A<DataAccess.Entities.Parcel>._))
                .Returns(parcel);
            
            Action reportParcelDelivery = () => _logic.ReportParcelDelivery(trackingId);

            reportParcelDelivery.Should().NotThrow<Exception>();
            parcel.FutureHops.Count.Should().Be(0);
            parcel.VisitedHops.Count.Should().Be(1);
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
        public void GivenAParcelWithFutureHops_WhenReportingParcelHop_ThenTheHopIsRemovedFromFutureHops()
        {
            const string trackingId = "my_trackingId";
            
            var hop = Builder<BusinessLogic.Entities.Hop>
                .CreateNew()
                .Build();
            
            var parcel = Builder<Parcel>
                .CreateNew()
                .With(p => p.FutureHops = new List<HopArrival>
                {
                    new HopArrival
                    {
                        DateTime = DateTime.Now, 
                        Hop = hop
                    }
                })
                .Build();

            A.CallTo(() => _mapper.Map<Parcel>(A<DataAccess.Entities.Parcel>._)).Returns(parcel);
            A.CallTo(() => _mapper.Map<BusinessLogic.Entities.Hop>(A<Hop>._)).Returns(hop);

            _logic.ReportParcelHop(trackingId, hop.Code);

            parcel.FutureHops.Count.Should().Be(0);
        }
        
        [Fact]
        public void GivenAParcelWithVisitedHops_WhenReportingParcelHop_ThenTheHopIsRemovedFromVisitedHops()
        {
            const string trackingId = "my_trackingId";
            
            var hop = Builder<BusinessLogic.Entities.Hop>
                .CreateNew()
                .Build();

            var hopArrivals = new List<HopArrival>
            {
                new HopArrival
                {
                    DateTime = DateTime.Now,
                    Hop = hop
                },
                new HopArrival
                {
                    DateTime = DateTime.Now,
                    Hop = hop
                },
                new HopArrival
                {
                    DateTime = DateTime.Now,
                    Hop = new Entities.Hop()
                }
            };

            var parcel = Builder<Parcel>
                .CreateNew()
                .With(p => p.VisitedHops = hopArrivals)
                .Build();

            A.CallTo(() => _mapper.Map<Parcel>(A<DataAccess.Entities.Parcel>._)).Returns(parcel);
            A.CallTo(() => _mapper.Map<BusinessLogic.Entities.Hop>(A<Hop>._)).Returns(hop);

            _logic.ReportParcelHop(trackingId, hop.Code);

            parcel.VisitedHops.Count.Should().Be(2);
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
            const string trackingId = "tracking_id";
            
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
            const string trackingId = "tracking_id";
            
            A.CallTo(() => _parcelRepository.GetByTrackingId(A<string>._)).Returns(null);

            Action trackParcel = () => _logic.TrackParcel(trackingId);

            trackParcel.Should().Throw<ParcelNotFoundException>();
        }
        
        [Fact]
        public void GivenADataAccessErrorOccurs_WhenTrackingParcel_ThenThrowABusinessException()
        {
            const string trackingId = "tracking_id";

            A.CallTo(() => _parcelRepository.GetByTrackingId(A<string>._)).Throws<DataAccessException>();

            Action trackParcel = () => _logic.TrackParcel(trackingId);

            trackParcel.Should().Throw<BusinessException>();
        }
    }
}