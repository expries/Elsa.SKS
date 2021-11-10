using System;
using AutoMapper;
using Elsa.SKS.Controllers;
using Elsa.SKS.MappingProfiles;
using Elsa.SKS.Package.BusinessLogic;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using FakeItEasy;
using FizzWare.NBuilder;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Elsa.SKS.Package.Services.Tests
{
    public class RecipientApiTests
    {
        private readonly RecipientApiController _controller;

        private readonly IParcelTrackingLogic _trackingLogic;

        private readonly IMapper _mapper;
        
        private readonly ILogger<RecipientApiController> _logger;

        public RecipientApiTests()
        {
            _trackingLogic = A.Fake<IParcelTrackingLogic>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<RecipientApiController>>();
            _controller = new RecipientApiController(_trackingLogic, _mapper, _logger);
        }
        
        [Fact]
        public void GivenAParcelExists_WhenTrackingTheParcel_ThenReturn200()
        {
            var parcel = Builder<Parcel>
                .CreateNew()
                .Build();

            var parcelDto = Builder<DTOs.Parcel>
                .CreateNew()
                .Build();
            
            A.CallTo(() => _trackingLogic.TrackParcel(A<string>._)).Returns(parcel);
            A.CallTo(() => _mapper.Map<DTOs.Parcel>(A<Parcel>._)).Returns(parcelDto);
            
            var actionResult = _controller.TrackParcel(parcel.TrackingId);
            
            actionResult.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public void GivenAParcelNotFoundExceptionsIsThrown_WhenTrackingAParcel_ThenReturn404()
        {
            const string trackingId = "tracking_id";
            
            A.CallTo(() => _trackingLogic.TrackParcel(A<string>._)).Throws<ParcelNotFoundException>();

            var actionResult = _controller.TrackParcel(trackingId);

            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public void GivenABusinessExceptionIsThrown_WhenTrackingAParcel_ThenReturn400()
        {
            const string trackingId = "tracking_id";
            
            A.CallTo(() => _trackingLogic.TrackParcel(A<string>._)).Throws<BusinessException>();

            var actionResult = _controller.TrackParcel(trackingId);
            
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
        
    }
}