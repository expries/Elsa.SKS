using Elsa.SKS.Controllers;
using Elsa.SKS.Package.Services.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elsa.SKS.Package.Services.Test
{
    public class LogisticPartnerApiTests
    {
        private readonly LogisticsPartnerApiController _controller;

        public LogisticPartnerApiTests()
        {
            _controller = new LogisticsPartnerApiController();
        }

        [Fact]
        public void GivenAParcelIsExpected_WhenTransitioningTheParcel_ThenReturn200()
        {
            const string trackingId = TestConstants.TrackingIdOfParcelThatIsTransferred;
            var parcel = new Parcel();
            var actionResult = _controller.TransitionParcel(parcel, trackingId);
            actionResult.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public void GivenAParcelIsNotExpected_WhenTransitioningTheParcel_ThenReturn400()
        {
            const string trackingId = TestConstants.TrackingIdOfParcelThatIsNotTransferred;
            var parcel = new Parcel();
            var actionResult = _controller.TransitionParcel(parcel, trackingId);
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}