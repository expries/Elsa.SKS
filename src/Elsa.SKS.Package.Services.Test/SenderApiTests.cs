using Elsa.SKS.Controllers;
using Elsa.SKS.Package.Services.DTOs;
using FluentAssertions;
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
            var typeAssert = actionResult.Should().BeOfType<CreatedResult>();
            var creationResult = typeAssert.Subject;
            creationResult.Location.Should().Be($"/{TestConstants.TrackingIdOfSubmittedParcel}");
        }
        
        [Fact]
        public void GivenANewInvalidParcel_WhenSubmittingTheParcel_ThenReturn400()
        {
            var actionResult = _controller.SubmitParcel(new Parcel(){ Weight = -1 });
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}