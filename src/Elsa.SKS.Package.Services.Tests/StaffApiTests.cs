using AutoMapper;
using Elsa.SKS.Controllers;
using Elsa.SKS.MappingProfiles;
using Elsa.SKS.Package.BusinessLogic;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elsa.SKS.Package.Services.Tests
{
    public class StaffApiTests
    {
        [Fact]
        public void GivenAParcelExists_WhenParcelDeliveryIsReported_ThenReturn200()
        {
            var parcelTracking = A.Fake<IParcelTracking>();
            
            A.CallTo(() => parcelTracking.ReportParcelDelivery(A<string>._))
                .Returns(new BusinessLogic.Entities.Parcel());

            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<ParcelProfile>()));
            var controller = new StaffApiController(parcelTracking, mapper);
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;
            
            var actionResult = controller.ReportParcelDelivery(trackingId);
            actionResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GivenAParcelNotFoundExceptionIsThrown_WhenParcelDeliveryIsReported_ThenReturn404()
        {
            var parcelTracking = A.Fake<IParcelTracking>();
            
            A.CallTo(() => parcelTracking.ReportParcelDelivery(A<string>._))
                .Throws<ParcelNotFoundException>();

            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<ParcelProfile>()));
            var controller = new StaffApiController(parcelTracking, mapper);
            const string trackingId = TestConstants.TrackingIdOfNonExistentParcel;
            
            var actionResult = controller.ReportParcelDelivery(trackingId);
            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public void GivenABusinessExceptionIsThrown_WhenParcelDeliveryIsReported_ThenReturn400()
        {
            var parcelTracking = A.Fake<IParcelTracking>();
            
            A.CallTo(() => parcelTracking.ReportParcelDelivery(A<string>._))
                .Throws<BusinessException>();
            
            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<ParcelProfile>()));
            var controller = new StaffApiController(parcelTracking, mapper);
            const string trackingId = TestConstants.TrackingIdOfParcelThatCanNotBeDelivered;
            
            var actionResult = controller.ReportParcelDelivery(trackingId);
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void GivenAParcelNotFoundExceptionIsThrown_WhenParcelHopIsReported_ThenReturn404()
        {
            var parcelTracking = A.Fake<IParcelTracking>();
            
            A.CallTo(() => parcelTracking.ReportParcelHop(A<string>._, A<string>._))
                .Throws<ParcelNotFoundException>();
            
            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<ParcelProfile>()));
            var controller = new StaffApiController(parcelTracking, mapper);
            const string trackingId = TestConstants.TrackingIdOfNonExistentParcel;
            const string code = TestConstants.ExistentHopCode;
            
            var actionResult = controller.ReportParcelHop(trackingId, code);
            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public void GivenAHopNotFoundExceptionIsThrown_WhenParcelHopIsReported_ThenReturn404()
        {
            var parcelTracking = A.Fake<IParcelTracking>();
            
            A.CallTo(() => parcelTracking.ReportParcelHop(A<string>._, A<string>._))
                .Throws<ParcelNotFoundException>();
            
            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<ParcelProfile>()));
            var controller = new StaffApiController(parcelTracking, mapper);
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;
            const string code = TestConstants.NonExistentHopCode;
            
            var actionResult = controller.ReportParcelHop(trackingId, code);
            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public void GivenABusinessExceptionIsThrown_WhenParcelHopIsReported_ThenReturn400()
        {
            var parcelTracking = A.Fake<IParcelTracking>();
            
            A.CallTo(() => parcelTracking.ReportParcelHop(A<string>._, A<string>._))
                .Throws<BusinessException>();

            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<ParcelProfile>()));
            var controller = new StaffApiController(parcelTracking, mapper);
            const string trackingId = TestConstants.TrackingIdOfParcelThatCanNotBeReported;
            const string code = TestConstants.ExistentHopCode;
            
            var actionResult = controller.ReportParcelHop(trackingId, code);
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
        
        [Fact]
        public void GivenAParcelExists_WhenParcelHopIsReported_ThenReturn200()
        {
            var parcelTracking = A.Fake<IParcelTracking>();

            A.CallTo(() => parcelTracking.ReportParcelHop(A<string>._, A<string>._))
                .DoesNothing();

            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<ParcelProfile>()));
            var controller = new StaffApiController(parcelTracking, mapper);
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;
            const string code = TestConstants.ExistentHopCode;
            
            var actionResult = controller.ReportParcelHop(trackingId, code);
            actionResult.Should().BeOfType<OkResult>();
        }
    }
}