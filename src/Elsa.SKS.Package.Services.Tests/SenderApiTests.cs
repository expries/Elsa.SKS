using AutoMapper;
using Elsa.SKS.Controllers;
using Elsa.SKS.MappingProfiles;
using Elsa.SKS.Package.BusinessLogic;
using Elsa.SKS.Package.Services.DTOs;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elsa.SKS.Package.Services.Tests
{
    public class SenderApiTests
    {
        private readonly SenderApiController _controller;

        public SenderApiTests()
        {
            var parcelRegistration = new ParcelRegistration();
            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<ParcelProfile>()));
            _controller = new SenderApiController(parcelRegistration, mapper);
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
            var actionResult = _controller.SubmitParcel(new Parcel { Weight = -1 });
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