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
    public class LogisticPartnerApiTests
    {
        private readonly LogisticsPartnerApiController _controller;

        private readonly IParcelRegistrationLogic _registrationLogic;
        
        private readonly IMapper _mapper;
        
        public LogisticPartnerApiTests()
        {
            _registrationLogic = A.Fake<IParcelRegistrationLogic>();
            _mapper = A.Fake<IMapper>();
            _controller = new LogisticsPartnerApiController(_registrationLogic, _mapper);
        }
        
        [Fact]
        public void GivenAParcelIsExpected_WhenTransitioningTheParcel_ThenReturn200()
        {
            var parcel = Builder<BusinessLogic.Entities.Parcel>
                .CreateNew()
                .Build();

            var parcelDto = Builder<Parcel>
                .CreateNew()
                .Build();

            A.CallTo(() => _registrationLogic.TransitionParcel(A<BusinessLogic.Entities.Parcel>._, A<string>._))
                .Returns(parcel);
            
            var actionResult = _controller.TransitionParcel(parcelDto, parcel.TrackingId);
            actionResult.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public void GivenAnTransferExceptionsIsThrown_WhenTransitioningAParcel_ThenReturn400()
        {
            const string trackingId = "tracking_id";
            
            var parcelDto = Builder<Parcel>
                .CreateNew()
                .Build();
            
            A.CallTo(() => _registrationLogic.TransitionParcel(A<BusinessLogic.Entities.Parcel>._, A<string>._))
                .Throws<TransferException>();

            var actionResult = _controller.TransitionParcel(parcelDto, trackingId);
            
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