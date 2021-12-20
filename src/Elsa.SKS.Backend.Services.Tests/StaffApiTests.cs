using AutoMapper;
using Elsa.SKS.Controllers;
using Elsa.SKS.Backend.BusinessLogic.Exceptions;
using Elsa.SKS.Backend.BusinessLogic.Interfaces;
using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Elsa.SKS.Backend.Services.Tests
{
    public class StaffApiTests
    {
        private readonly StaffApiController _controller;

        private readonly IParcelTrackingLogic _trackingLogic;

        private readonly IMapper _mapper;
        
        private readonly ILogger<StaffApiController> _logger;

        public StaffApiTests()
        {
            _trackingLogic = A.Fake<IParcelTrackingLogic>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<StaffApiController>>();
            _controller = new StaffApiController(_trackingLogic, _mapper, _logger);
        }
        
        [Fact]
        public void GivenAParcelExists_WhenParcelDeliveryIsReported_ThenReturn200()
        {
            const string trackingId = "tracking_id";

            A.CallTo(() => _trackingLogic.ReportParcelDelivery(A<string>._))
                .DoesNothing();
            
            var actionResult = _controller.ReportParcelDelivery(trackingId);
            actionResult.Should().BeOfType<OkResult>();
        }

        [Fact]
        public void GivenAParcelNotFoundExceptionIsThrown_WhenParcelDeliveryIsReported_ThenReturn404()
        {
            const string trackingId = "tracking_id";

            A.CallTo(() => _trackingLogic.ReportParcelDelivery(A<string>._))
                .Throws<ParcelNotFoundException>();

            var actionResult = _controller.ReportParcelDelivery(trackingId);
            
            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public void GivenABusinessExceptionIsThrown_WhenParcelDeliveryIsReported_ThenReturn400()
        {
            const string trackingId = "tracking_id";
            
            A.CallTo(() => _trackingLogic.ReportParcelDelivery(A<string>._))
                .Throws<BusinessException>();

            var actionResult = _controller.ReportParcelDelivery(trackingId);
            
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }

        [Fact]
        public void GivenAParcelNotFoundExceptionIsThrown_WhenParcelHopIsReported_ThenReturn404()
        {
            const string trackingId = "tracking_id";
            const string hopCode = "hop_code";
            
            A.CallTo(() => _trackingLogic.ReportParcelHop(A<string>._, A<string>._))
                .Throws<ParcelNotFoundException>();

            var actionResult = _controller.ReportParcelHop(trackingId, hopCode);
            
            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public void GivenAHopNotFoundExceptionIsThrown_WhenParcelHopIsReported_ThenReturn404()
        {
            const string trackingId = "tracking_id";
            const string hopCode = "hop_code";

            A.CallTo(() => _trackingLogic.ReportParcelHop(A<string>._, A<string>._))
                .Throws<ParcelNotFoundException>();

            var actionResult = _controller.ReportParcelHop(trackingId, hopCode);
            
            actionResult.Should().BeOfType<NotFoundResult>();
        }
        
        [Fact]
        public void GivenABusinessExceptionIsThrown_WhenParcelHopIsReported_ThenReturn400()
        {
            const string trackingId = "tracking_id";
            const string hopCode = "hop_code";
            
            A.CallTo(() => _trackingLogic.ReportParcelHop(A<string>._, A<string>._))
                .Throws<BusinessException>();

            var actionResult = _controller.ReportParcelHop(trackingId, hopCode);
            
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
        
        [Fact]
        public void GivenAParcelExists_WhenParcelHopIsReported_ThenReturn200()
        {
            const string trackingId = "tracking_id";
            const string hopCode = "hop_code";
            
            A.CallTo(() => _trackingLogic.ReportParcelHop(A<string>._, A<string>._))
                .DoesNothing();

            var actionResult = _controller.ReportParcelHop(trackingId, hopCode);
            
            actionResult.Should().BeOfType<OkResult>();
        }
    }
}