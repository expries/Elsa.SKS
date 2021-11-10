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
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Elsa.SKS.Package.Services.Tests
{
    public class SenderApiTests
    {
        private readonly SenderApiController _controller;

        private readonly IParcelRegistrationLogic _registrationLogic;

        private readonly IMapper _mapper;
        
        private readonly ILogger<SenderApiController> _logger;

        public SenderApiTests()
        {
            _registrationLogic = A.Fake<IParcelRegistrationLogic>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<SenderApiController>>();
            _controller = new SenderApiController(_registrationLogic, _mapper, _logger);
        }
        
        [Fact]
        public void GivenANewParcel_WhenSubmittingTheParcel_ThenReturn201()
        {
            var parcel = Builder<BusinessLogic.Entities.Parcel>
                .CreateNew()
                .Build();

            var parcelDto = Builder<Parcel>
                .CreateNew()
                .Build();
            
            var newParcelInfo = Builder<NewParcelInfo>
                .CreateNew()
                .With(p => p.TrackingId = parcel.TrackingId)
                .Build();

            A.CallTo(() => _registrationLogic.SubmitParcel(A<BusinessLogic.Entities.Parcel>._))
                .Returns(parcel);

            A.CallTo(() => _mapper.Map<NewParcelInfo>(A<BusinessLogic.Entities.Parcel>._))
                .Returns(newParcelInfo);

            var actionResult = _controller.SubmitParcel(parcelDto);
            
            var typeAssertion = actionResult.Should().BeOfType<CreatedResult>();
            var creationResult = typeAssertion.Subject;
            creationResult.Location.Should().Be($"/{newParcelInfo.TrackingId}");
        }
        
        [Fact]
        public void GivenABusinessExceptionIsThrown_WhenSubmittingAParcel_ThenReturn400()
        {
            var parcelDto = Builder<Parcel>
                .CreateNew()
                .Build();
            
            A.CallTo(() => _registrationLogic.SubmitParcel(A<BusinessLogic.Entities.Parcel>._))
                .Throws<BusinessException>();

            var actionResult = _controller.SubmitParcel(parcelDto);
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