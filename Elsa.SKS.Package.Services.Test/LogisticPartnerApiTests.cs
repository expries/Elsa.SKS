using System;
using Elsa.SKS.Controllers;
using Elsa.SKS.Package.Services.DTOs;
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
            // arrange
            const string trackingId = TestConstants.TrackingIdOfParcelThatIsTransferred;
            var parcel = new Parcel();
            
            // act
            var actionResult = _controller.TransitionParcel(parcel, trackingId);
            
            // assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(200, statusCodeResult.StatusCode);
        }
        
        [Fact]
        public void GivenAParcelIsNotExpected_WhenTransitioningTheParcel_ThenReturn400()
        {
            // arrange
            const string trackingId = TestConstants.TrackingIdOfParcelThatIsNotTransferred;
            var parcel = new Parcel();
            
            // act
            var actionResult = _controller.TransitionParcel(parcel, trackingId);
            
            // assert
            var statusCodeResult = Assert.IsType<StatusCodeResult>(actionResult);
            Assert.Equal(400, statusCodeResult.StatusCode);
        }
    }
}