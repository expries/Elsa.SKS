using Elsa.SKS.Controllers;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elsa.SKS.Package.Services.Test
{
    public class RecipientApiTests
    {
        private readonly RecipientApiController _controller;

        public RecipientApiTests()
        {
            _controller = new RecipientApiController();
        }

        [Fact]
        public void GivenAParcelExists_WhenTrackingTheParcel_ThenReturn200()
        {
            const string trackingId = TestConstants.TrackingIdOfExistentParcel;
            var actionResult = _controller.TrackParcel(trackingId);
            Assert.IsType<OkObjectResult>(actionResult);
        }
        
        [Fact]
        public void GivenAParcelDoesNotExist_WhenTrackingTheParcel_ThenReturn404()
        {
            const string trackingId = TestConstants.TrackingIdOfNonExistentParcel;
            var actionResult = _controller.TrackParcel(trackingId);
            Assert.IsType<NotFoundResult>(actionResult);
        }
        
        [Fact]
        public void GivenAParcelCanNotBeTracked_WhenTrackingTheParcel_ThenReturn400()
        {
            const string trackingId = TestConstants.TrackingIdOfParcelThatCanNotBeTracked;
            var actionResult = _controller.TrackParcel(trackingId);
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }
    }
}