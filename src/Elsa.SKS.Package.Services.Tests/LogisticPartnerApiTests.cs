using Elsa.SKS.Controllers;
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
        [Fact]
        public void GivenAParcelIsExpected_WhenTransitioningTheParcel_ThenReturn200()
        {
            var parcelRegistration = A.Fake<IParcelRegistration>();
            
            A.CallTo(() => parcelRegistration.TransitionParcel(A<BusinessLogic.Entities.Parcel>._, A<string>._))
                .Returns(new BusinessLogic.Entities.Parcel());
            
            var controller = new LogisticsPartnerApiController(parcelRegistration);
            var parcel = Builder<Parcel>.CreateNew().Build();
            const string trackingId = TestConstants.TrackingIdOfParcelThatIsTransferred;

            var actionResult = controller.TransitionParcel(parcel, trackingId);
            actionResult.Should().BeOfType<OkObjectResult>();
        }
        
        [Fact]
        public void GivenAnTransferExceptionsIsThrown_WhenTransitioningAParcel_ThenReturn400()
        {
            var parcelRegistration = A.Fake<IParcelRegistration>();
            
            A.CallTo(() => parcelRegistration.TransitionParcel(A<BusinessLogic.Entities.Parcel>._, A<string>._))
                .Throws<TransferException>();
            
            var controller = new LogisticsPartnerApiController(parcelRegistration);
            var parcel = Builder<Parcel>.CreateNew().Build();
            const string trackingId = TestConstants.TrackingIdOfParcelThatIsNotTransferred;
            
            var actionResult = controller.TransitionParcel(parcel, trackingId);
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}