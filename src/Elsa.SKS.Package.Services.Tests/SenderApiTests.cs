using AutoMapper;
using Elsa.SKS.Controllers;
using Elsa.SKS.MappingProfiles;
using Elsa.SKS.Package.BusinessLogic;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.Services.DTOs;
using FakeItEasy;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Xunit;

namespace Elsa.SKS.Package.Services.Tests
{
    public class SenderApiTests
    {
        [Fact]
        public void GivenANewParcel_WhenSubmittingTheParcel_ThenReturn201()
        {
            var parcelRegistration = A.Fake<IParcelRegistration>();
            
            A.CallTo(() => parcelRegistration.SubmitParcel(A<BusinessLogic.Entities.Parcel>._))
                .Returns(new BusinessLogic.Entities.Parcel { TrackingId = TestConstants.TrackingIdOfSubmittedParcel });

            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<ParcelProfile>()));
            var controller = new SenderApiController(parcelRegistration, mapper);
            var parcel = Builder<Parcel>.CreateNew().Build();
            
            var actionResult = controller.SubmitParcel(parcel);
            var typeAssertion = actionResult.Should().BeOfType<CreatedResult>();
            var creationResult = typeAssertion.Subject;
            creationResult.Location.Should().Be($"/{TestConstants.TrackingIdOfSubmittedParcel}");
        }
        
        [Fact]
        public void GivenABusinessExceptionIsThrown_WhenSubmittingAParcel_ThenReturn400()
        {
            var parcelRegistration = A.Fake<IParcelRegistration>();
            
            A.CallTo(() => parcelRegistration.SubmitParcel(A<BusinessLogic.Entities.Parcel>._))
                .Throws<BusinessException>();

            var mapper = new Mapper(new MapperConfiguration(c => c.AddProfile<ParcelProfile>()));
            var controller = new SenderApiController(parcelRegistration, mapper);
            var parcel = Builder<Parcel>.CreateNew().Build();
            
            var actionResult = controller.SubmitParcel(parcel);
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