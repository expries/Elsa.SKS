using System;
using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Entities.Enums;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
using Elsa.SKS.Package.ServiceAgents.Entities;
using Elsa.SKS.Package.ServiceAgents.Interfaces;
using FakeItEasy;
using FizzWare.NBuilder;
using FluentAssertions;
using FluentValidation;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;
using Xunit;
using Truck = Elsa.SKS.Package.DataAccess.Entities.Truck;

namespace Elsa.SKS.Package.BusinessLogic.Tests
{
    public class ParcelRegistrationLogicTests
    {
        private readonly IParcelRegistrationLogic _logic;

        private readonly IParcelRepository _parcelRepository;
        
        private readonly IHopRepository _hopRepository;
        
        private readonly IGeocodingAgent _geocodingAgent;

        private readonly IValidator<Parcel> _parcelValidator;
        
        private readonly IMapper _mapper;
        
        private readonly ILogger<ParcelRegistrationLogic> _logger;

        private static Polygon SamplePolygon => GetSamplePolygon();
        
        private static Point PointInsideSamplePolygon => new Point(48.1807385, 11.9091797);

        public ParcelRegistrationLogicTests()
        {
            _parcelRepository = A.Fake<IParcelRepository>();
            _parcelValidator = A.Fake<IValidator<Parcel>>();
            _mapper = A.Fake<IMapper>();
            _logger = A.Fake<ILogger<ParcelRegistrationLogic>>();
            _geocodingAgent = A.Fake<IGeocodingAgent>();
            _hopRepository = A.Fake<IHopRepository>();
            _logic = new ParcelRegistrationLogic(_parcelRepository, _parcelValidator, _mapper, _logger, _geocodingAgent, _hopRepository);
        }
        
        [Fact]
        public void GivenAValidParcel_WhenTransitioningTheParcel_ThenReturnParcelWithStatusPickup()
        {
            var validationResult = new ValidationResult();

            var parcel = Builder<Parcel>
                .CreateNew()
                .Build();
            
            var parcelEntity = Builder<DataAccess.Entities.Parcel>
                .CreateNew()
                .Build();
            
            var geoLocation = Builder<Geolocation>
                .CreateNew()
                .With(_ => _.Latitude = PointInsideSamplePolygon.Y)
                .With(_ => _.Longitude = PointInsideSamplePolygon.X)
                .Build();

            var truck = Builder<Truck>
                .CreateNew()
                .With(_ => _.GeoRegion = SamplePolygon)
                .Build();

            var trucks = new List<Truck> { truck };

            A.CallTo(() => _parcelValidator.Validate(A<Parcel>._)).Returns(validationResult);
            A.CallTo(() => _parcelRepository.GetByTrackingId(A<string>._)).Returns(null);
            A.CallTo(() => _geocodingAgent.GeocodeAddress(A<Address>._)).Returns(geoLocation);
            A.CallTo(() => _hopRepository.GetAllTrucks()).Returns(trucks);
            A.CallTo(() => _parcelRepository.Create(A<DataAccess.Entities.Parcel>._)).Returns(parcelEntity);
            A.CallTo(() => _mapper.Map<Parcel>(A<DataAccess.Entities.Parcel>._)).Returns(parcel);
            
            var transitionParcel = _logic.TransitionParcel(parcel, parcel.TrackingId);
            
            transitionParcel.State.Should().Be(ParcelState.Pickup);
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
        public void GivenAValidParcel_WhenSubmittingParcel_ThenReturnParcelWithStatusPickup()
        {
            var validationResult = new ValidationResult();
            
            var user = Builder<User>
                .CreateNew()
                .Build();
            
            var parcel = Builder<Parcel>
                .CreateNew()
                .With(_ => _.Sender = user)
                .With(_ => _.Recipient = user)
                .Build();

            var geoLocation = Builder<Geolocation>
                .CreateNew()
                .With(_ => _.Latitude = PointInsideSamplePolygon.Y)
                .With(_ => _.Longitude = PointInsideSamplePolygon.X)
                .Build();

            var truck = Builder<Truck>
                .CreateNew()
                .With(_ => _.GeoRegion = SamplePolygon)
                .Build();

            var trucks = new List<Truck> { truck };

            A.CallTo(() => _parcelValidator.Validate(A<Parcel>._)).Returns(validationResult);
            A.CallTo(() => _geocodingAgent.GeocodeAddress(A<Address>._)).Returns(geoLocation);
            A.CallTo(() => _hopRepository.GetAllTrucks()).Returns(trucks);
            A.CallTo(() => _mapper.Map<Parcel>(A<DataAccess.Entities.Parcel>._)).Returns(parcel);
            
            var submittedParcel = _logic.SubmitParcel(parcel);
            
            submittedParcel.State.Should().Be(ParcelState.Pickup);
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
            
            var user = Builder<User>
                .CreateNew()
                .Build();
            
            var parcel = Builder<Parcel>
                .CreateNew()
                .With(_ => _.Sender = user)
                .With(_ => _.Recipient = user)
                .Build();
            
            var geoLocation = Builder<Geolocation>
                .CreateNew()
                .With(_ => _.Latitude = PointInsideSamplePolygon.Y)
                .With(_ => _.Longitude = PointInsideSamplePolygon.X)
                .Build();

            var truck = Builder<Truck>
                .CreateNew()
                .With(_ => _.GeoRegion = SamplePolygon)
                .Build();

            var trucks = new List<Truck> { truck };

            A.CallTo(() => _parcelValidator.Validate(A<Parcel>._)).Returns(validationResult);
            A.CallTo(() => _geocodingAgent.GeocodeAddress(A<Address>._)).Returns(geoLocation);
            A.CallTo(() => _hopRepository.GetAllTrucks()).Returns(trucks);
            A.CallTo(() => _parcelRepository.Create(A<DataAccess.Entities.Parcel>._)).Throws<DataAccessException>();
            
            Action submitParcel = () => _logic.SubmitParcel(parcel);

            submitParcel.Should().Throw<BusinessException>();
        }
        
        private static Polygon GetSamplePolygon()
        {
            // first longitude then latitude
            var coordinates = new List<double[]>
                {
                    new[] { 48.5747899, 10.8764648 },
                    new[] { 48.5166043, 12.8979492 },
                    new[] { 47.916342, 12.9199219 },
                    new[] { 47.8131545, 11.0083008 },
                    new[] { 48.5747899, 10.8764648 }
                }
                .Select(_ => new Coordinate(_[0], _[1]))
                .ToArray();

            var linearRing = new LinearRing(coordinates);
            return new Polygon(linearRing);
        }
    }
}