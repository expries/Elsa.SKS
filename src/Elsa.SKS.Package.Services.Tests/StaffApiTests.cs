using Elsa.SKS.Controllers;
using Elsa.SKS.Package.BusinessLogic;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elsa.SKS.Package.Services.Tests
{
    public class StaffApiTests
    {
        private readonly StaffApiController _controller;

        public StaffApiTests()
        {
            var parcelTracking = new ParcelTracking();
            _controller = new StaffApiController(parcelTracking);
        }

        [Fact]
        public void GivenAParcelExists_WhenParcelDeliveryIsReported_ThenReturn200()
        {
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;
            var actionResult = _controller.ReportParcelDelivery(trackingId);
            actionResult.Should().BeOfType<OkObjectResult>();
        }

        [Fact]
        public void GivenAParcelDoesNotExist_WhenParcelDeliveryIsReported_ThenReturn404()
        {
            const string trackingId = TestConstants.TrackingIdOfNonExistentParcel;
            var actionResult = _controller.ReportParcelDelivery(trackingId);
            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public void GivenAParcelCanNotBeDelivered_WhenParcelDeliveryIsReported_ThenReturn400()
        {
            const string trackingId = TestConstants.TrackingIdOfParcelThatCanNotBeDelivered;
            var actionResult = _controller.ReportParcelDelivery(trackingId);
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void GivenAParcelDoesNotExist_WhenParcelHopIsReported_ThenReturn404()
        {
            const string trackingId = TestConstants.TrackingIdOfNonExistentParcel;
            const string code = TestConstants.ExistentHopCode;
            var actionResult = _controller.ReportParcelHop(trackingId, code);
            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public void GivenAHopCodeDoesNotExist_WhenParcelHopIsReported_ThenReturn404()
        {
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;
            const string code = TestConstants.NonExistentHopCode;
            var actionResult = _controller.ReportParcelHop(trackingId, code);
            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public void GivenAParcelCanNotBeReported_WhenParcelHopIsReported_ThenReturn400()
        {
            const string trackingId = TestConstants.TrackingIdOfParcelThatCanNotBeReported;
            const string code = TestConstants.ExistentHopCode;
            var actionResult = _controller.ReportParcelHop(trackingId, code);
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
        
        [Fact]
        public void GivenAParcelExists_WhenParcelHopIsReported_ThenReturn200()
        {
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;
            const string code = TestConstants.ExistentHopCode;
            var actionResult = _controller.ReportParcelHop(trackingId, code);
            actionResult.Should().BeOfType<OkResult>();
        }
    }
}