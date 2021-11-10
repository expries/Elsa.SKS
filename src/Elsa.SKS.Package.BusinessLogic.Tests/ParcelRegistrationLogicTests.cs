using System;
using System.Collections.Generic;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
using FakeItEasy;
using FizzWare.NBuilder;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Elsa.SKS.Package.BusinessLogic.Tests
{
    public class ParcelRegistrationLogicTests
    {
        private readonly IParcelRegistrationLogic _logic;

        private readonly IParcelRepository _parcelRepository;
        
        private readonly IValidator<Parcel> _parcelValidator;
        
        private readonly IMapper _mapper;
        
        private readonly ILogger<ParcelRegistrationLogic> _logger;

        public ParcelRegistrationLogicTests()
        {
            _parcelRepository = A.Fake<IParcelRepository>();
            _parcelValidator = A.Fake<IValidator<Parcel>>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<ParcelRegistrationLogic>>();
            _logic = new ParcelRegistrationLogic(_parcelRepository, _parcelValidator, _mapper, _logger);
        }
        
        [Fact]
        public void GivenAValidParcel_WhenTransitioningTheParcel_ThenReturnParcel()
        {
            var validationResult = new ValidationResult();

            var parcel = Builder<Parcel>
                .CreateNew()
                .Build();
            
            var parcelEntity = Builder<DataAccess.Entities.Parcel>
                .CreateNew()
                .Build();

            A.CallTo(() => _parcelValidator.Validate(A<Parcel>._)).Returns(validationResult);
            A.CallTo(() => _parcelRepository.GetByTrackingId(A<string>._)).Returns(null);
            A.CallTo(() => _parcelRepository.Create(A<DataAccess.Entities.Parcel>._)).Returns(parcelEntity);
            A.CallTo(() => _mapper.Map<Parcel>(A<DataAccess.Entities.Parcel>._)).Returns(parcel);
            
            var parcelReturned = _logic.TransitionParcel(parcel, parcel.TrackingId);
            
            parcelReturned.Should().Be(parcel);
        }
        
        [Fact]
        public void GivenParcelValidationFails_WhenTransitioningTheParcel_ThenThrowATransactionException()
        {
            var validationFailure = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("property", "error")
            });

            var parcel = Builder<Parcel>
                .CreateNew()
                .Build();

            A.CallTo(() => _parcelValidator.Validate(A<Parcel>._)).Returns(validationFailure);
            A.CallTo(() => _parcelRepository.GetByTrackingId(A<string>._)).Returns(null);
            A.CallTo(() => _mapper.Map<Parcel>(A<DataAccess.Entities.Parcel>._)).Returns(parcel);

            Action transition = () =>_logic.TransitionParcel(parcel, parcel.TrackingId);

            transition.Should().Throw<TransferException>();
        }
        
        [Fact]
        public void GivenAParcelWithTheSameTrackingIdExists_WhenTransitioningTheParcel_ThenThrowATransferException()
        {
            var validationResult = new ValidationResult();

            var parcel = Builder<Parcel>
                .CreateNew()
                .Build();
            
            var parcelEntity = Builder<DataAccess.Entities.Parcel>
                .CreateNew()
                .Build();

            A.CallTo(() => _parcelValidator.Validate(A<Parcel>._)).Returns(validationResult);
            A.CallTo(() => _parcelRepository.GetByTrackingId(A<string>._)).Returns(parcelEntity);

            Action transition = () =>_logic.TransitionParcel(parcel, parcel.TrackingId);
            
            transition.Should().Throw<TransferException>();
        }
        
        [Fact]
        public void GivenADataAccessErrorOccurs_WhenTransitioningTheParcel_ThenThrowABusinessException()
        {
            var validationResult = new ValidationResult();

            var parcel = Builder<Parcel>
                .CreateNew()
                .Build();

            A.CallTo(() => _parcelValidator.Validate(A<Parcel>._)).Returns(validationResult);
            A.CallTo(() => _parcelRepository.GetByTrackingId(A<string>._)).Returns(null);
            A.CallTo(() => _parcelRepository.GetByTrackingId(A<string>._)).Throws<DataAccessException>();

            Action transition = () =>_logic.TransitionParcel(parcel, parcel.TrackingId);
            
            transition.Should().Throw<BusinessException>();
        }
        
        [Fact]
        public void GivenAValidParcel_WhenSubmittingTheParcel_ThenReturnTheParcel()
        {
            var validationResult = new ValidationResult();
            
            var parcel = Builder<Parcel>
                .CreateNew()
                .Build();
            
            var parcelEntity = Builder<DataAccess.Entities.Parcel>
                .CreateNew()
                .Build();
            
            A.CallTo(() => _parcelValidator.Validate(A<Parcel>._)).Returns(validationResult);
            A.CallTo(() => _parcelRepository.Create(A<DataAccess.Entities.Parcel>._)).Returns(parcelEntity);
            A.CallTo(() => _mapper.Map<Parcel>(A<DataAccess.Entities.Parcel>._)).Returns(parcel);

            var parcelReturned = _logic.SubmitParcel(parcel);

            parcelReturned.Should().Be(parcel);
        }
        
        [Fact]
        public void GivenParcelValidationFails_WhenSubmittingTheParcel_ThenThrowInvalidParcelException()
        {
            var validationFailure = new ValidationResult(new List<ValidationFailure>
            {
                new ValidationFailure("property", "error")
            });
            
            var parcel = Builder<Parcel>
                .CreateNew()
                .Build();
            
            A.CallTo(() => _parcelValidator.Validate(A<Parcel>._)).Returns(validationFailure);

            Action submitParcel = () =>  _logic.SubmitParcel(parcel);

            submitParcel.Should().Throw<InvalidParcelException>();
        }
        
        [Fact]
        public void GivenADataAccessErrorOccurs_WhenSubmittingTheParcel_ThenThrowABusinessException()
        {
            var validationResult = new ValidationResult();
            
            var parcel = Builder<Parcel>
                .CreateNew()
                .Build();

            A.CallTo(() => _parcelValidator.Validate(A<Parcel>._)).Returns(validationResult);
            A.CallTo(() => _parcelRepository.Create(A<DataAccess.Entities.Parcel>._)).Throws<DataAccessException>();
            
            Action submitParcel = () => _logic.SubmitParcel(parcel);

            submitParcel.Should().Throw<BusinessException>();
        }
    }
}