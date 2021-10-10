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
    public class RecipientApiTests
    {
        [Fact]
        public void GivenAParcelExists_WhenTrackingTheParcel_ThenReturn200()
        {
            var parcelTracking = A.Fake<IParcelTracking>();
            
            A.CallTo(() => parcelTracking.TrackParcel(A<string>._))
                .Returns(new BusinessLogic.Entities.Parcel());

            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<ParcelProfile>()));
            var controller = new RecipientApiController(parcelTracking, mapper);
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;
            
            var actionResult = controller.TrackParcel(trackingId);
            actionResult.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public void GivenAParcelNotFoundExceptionsIsThrown_WhenTrackingAParcel_ThenReturn404()
        {
            var parcelTracking = A.Fake<IParcelTracking>();
            
            A.CallTo(() => parcelTracking.TrackParcel(A<string>._))
                .Throws<ParcelNotFoundException>();

            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<ParcelProfile>()));
            var controller = new RecipientApiController(parcelTracking, mapper);
            const string trackingId = TestConstants.TrackingIdOfNonExistentParcel;
            
            var actionResult = controller.TrackParcel(trackingId);
            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public void GivenABusinessExceptionIsThrown_WhenTrackingAParcel_ThenReturn400()
        {
            var parcelTracking = A.Fake<IParcelTracking>();
            
            A.CallTo(() => parcelTracking.TrackParcel(A<string>._))
                .Throws<BusinessException>();

            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<ParcelProfile>()));
            var controller = new RecipientApiController(parcelTracking, mapper);
            const string trackingId = TestConstants.TrackingIdOfParcelThatCanNotBeTracked;
            
            var actionResult = controller.TrackParcel(trackingId);
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
        
    }
}