﻿using Elsa.SKS.Controllers;
using Elsa.SKS.Package.BusinessLogic;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.Services.DTOs;
using FakeItEasy;
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

            var controller = new SenderApiController(parcelRegistration);
            
            var actionResult = controller.SubmitParcel(new Parcel());
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

            var controller = new SenderApiController(parcelRegistration);
            var actionResult = controller.SubmitParcel(new Parcel());
            actionResult.Should().BeOfType<BadRequestObjectResult>();
        }
    }
}