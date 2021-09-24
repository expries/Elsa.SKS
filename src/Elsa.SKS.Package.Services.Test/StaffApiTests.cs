using Elsa.SKS.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elsa.SKS.Package.Services.Test
{
    public class StaffApiTests
    {
        private readonly StaffApiController _controller;

        public StaffApiTests()
        {
            _controller = new StaffApiController();
        }

        [Fact]
        public void GivenAParcelExists_WhenParcelDeliveryIsReported_ThenReturn200()
        {
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;
            var actionResult = _controller.ReportParcelDelivery(trackingId);
            Assert.IsType<OkObjectResult>(actionResult);
        }

        [Fact]
        public void GivenAParcelDoesNotExist_WhenParcelDeliveryIsReported_ThenReturn404()
        {
            const string trackingId = TestConstants.TrackingIdOfNonExistentParcel;
            var actionResult = _controller.ReportParcelDelivery(trackingId);
            Assert.IsType<NotFoundResult>(actionResult);
        }
        
        [Fact]
        public void GivenAParcelCanNotBeDelivered_WhenParcelDeliveryIsReported_ThenReturn400()
        {
            const string trackingId = TestConstants.TrackingIdOfParcelThatCanNotBeDelivered;
            var actionResult = _controller.ReportParcelDelivery(trackingId);
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }

        [Fact]
        public void GivenAParcelDoesNotExist_WhenParcelHopIsReported_ThenReturn404()
        {
            const string trackingId = TestConstants.TrackingIdOfNonExistentParcel;
            const string code = TestConstants.ExistentHopCode;
            var actionResult = _controller.ReportParcelHop(trackingId, code);
            Assert.IsType<NotFoundResult>(actionResult);
        }
        
        [Fact]
        public void GivenAHopCodeDoesNotExist_WhenParcelHopIsReported_ThenReturn404()
        {
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;
            const string code = TestConstants.NonExistentHopCode;
            var actionResult = _controller.ReportParcelHop(trackingId, code);
            Assert.IsType<NotFoundResult>(actionResult);
        }
        
        [Fact]
        public void GivenAParcelCanNotBeReported_WhenParcelHopIsReported_ThenReturn400()
        {
            const string trackingId = TestConstants.TrackingIdOfParcelThatCanNotBeReported;
            const string code = TestConstants.ExistentHopCode;
            var actionResult = _controller.ReportParcelHop(trackingId, code);
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }
        
        [Fact]
        public void GivenAParcelExists_WhenParcelHopIsReported_ThenReturn200()
        {
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;
            const string code = TestConstants.ExistentHopCode;
            var actionResult = _controller.ReportParcelHop(trackingId, code);
            Assert.IsType<OkResult>(actionResult);
        }
    }
}