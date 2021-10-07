using AutoMapper;
using Elsa.SKS.Controllers;
using Elsa.SKS.Package.BusinessLogic;
using Elsa.SKS.Package.Services.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elsa.SKS.Package.Services.Tests
{
    public class LogisticPartnerApiTests
    {
        private readonly LogisticsPartnerApiController _controller;
        public LogisticPartnerApiTests()
        {
            var parcelRegistration = new ParcelRegistration();
            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<ParcelProfile>()));
            _controller = new LogisticsPartnerApiController(parcelRegistration, mapper);
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
        
        [Fact]
        public void AutomapperConfigurationTester()
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddProfile(new ParcelProfile()));

            configuration.AssertConfigurationIsValid();
        }
    }
}