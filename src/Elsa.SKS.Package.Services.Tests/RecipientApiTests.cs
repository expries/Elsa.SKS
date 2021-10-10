using AutoMapper;
using Elsa.SKS.Controllers;
using Elsa.SKS.MappingProfiles;
using Elsa.SKS.Package.BusinessLogic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elsa.SKS.Package.Services.Tests
{
    public class RecipientApiTests
    {
        private readonly RecipientApiController _controller;

        public RecipientApiTests()
        {
            var parcelTracking = new ParcelTracking();
            _controller = new RecipientApiController(parcelTracking);
        }

        [Fact]
        public void GivenAParcelExists_WhenTrackingTheParcel_ThenReturn200()
        {
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;
            var actionResult = _controller.TrackParcel(trackingId);
            actionResult.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public void GivenAParcelDoesNotExist_WhenTrackingTheParcel_ThenReturn404()
        {
            const string trackingId = TestConstants.TrackingIdOfNonExistentParcel;
            var actionResult = _controller.TrackParcel(trackingId);
            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public void GivenAParcelCanNotBeTracked_WhenTrackingTheParcel_ThenReturn400()
        {
            const string trackingId = TestConstants.TrackingIdOfParcelThatCanNotBeTracked;
            var actionResult = _controller.TrackParcel(trackingId);
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
        
    }
}