using Elsa.SKS.Controllers;
using Elsa.SKS.Package.Services.DTOs;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elsa.SKS.Package.Services.Test
{
    public class SenderApiTests
    {
        private readonly SenderApiController _controller;

        public SenderApiTests()
        {
            _controller = new SenderApiController();
        }

        [Fact]
        public void GivenANewParcel_WhenSubmittingTheParcel_ThenReturn201()
        {
            var actionResult = _controller.SubmitParcel(new Parcel());
            var createdResult = Assert.IsType<CreatedResult>(actionResult);
            Assert.Equal("/" + TestConstants.TrackingIdOfSubmittedParcel, createdResult.Location);
        }
        
        [Fact]
        public void GivenANewUnvalidParcel_WhenSubmittingTheParcel_ThenReturn400()
        {
            var actionResult = _controller.SubmitParcel(new Parcel(){ Weight = -1 });
            Assert.IsType<BadRequestObjectResult>(actionResult);
        }
    }
}