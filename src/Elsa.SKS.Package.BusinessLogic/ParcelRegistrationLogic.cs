using System;
using System.Linq;
using System.Text;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
using Elsa.SKS.Package.ServiceAgents.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;

namespace Elsa.SKS.Package.BusinessLogic
{
    public class ParcelRegistrationLogic : IParcelRegistrationLogic
    {
        private readonly IParcelRepository _parcelRepository;

        private readonly IHopRepository _hopRepository;
        
        private readonly IValidator<Parcel> _parcelValidator;

        private readonly IMapper _mapper;
        
        private readonly ILogger<ParcelRegistrationLogic> _logger;

        private readonly IGeocodingAgent _geocodingAgent;

        public ParcelRegistrationLogic(IParcelRepository parcelRepository, IValidator<Parcel> parcelValidator, IMapper mapper, ILogger<ParcelRegistrationLogic> logger, IGeocodingAgent geocodingAgent, IHopRepository hopRepository)
        {
            _parcelRepository = parcelRepository;
            _parcelValidator = parcelValidator;
            _mapper = mapper;
            _logger = logger;
            _geocodingAgent = geocodingAgent;
            _hopRepository = hopRepository;
        }
    
        public Parcel TransitionParcel(Parcel parcel, string trackingId)
        {
            var validation = _parcelValidator.Validate(parcel);

            if (!validation.IsValid)
            {
                _logger.LogDebug("Validation failed");
                throw new TransferException(validation.ToString(" "));
            }
            
            try
            {
                var existingParcel = _parcelRepository.GetByTrackingId(trackingId);

                if (existingParcel is not null)
                {
                    _logger.LogInformation("A parcel with this tracking Id is already being tracked");
                    throw new TransferException("A parcel with this tracking Id is already being tracked.");
                }
            
                var parcelDal = _mapper.Map<Elsa.SKS.Package.DataAccess.Entities.Parcel>(parcel);
                var parcelEntity = _parcelRepository.Create(parcelDal);
                var result = _mapper.Map<Parcel>(parcelEntity);
                return result;
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Database error");
                throw new BusinessException("A database error has occurred.", ex);
            }
        }

        public Parcel SubmitParcel(Parcel parcel)
        {
            var validation = _parcelValidator.Validate(parcel);

            if (!validation.IsValid)
            {
                _logger.LogDebug("Validation failed");
                throw new InvalidParcelException(validation.ToString(" "));
            }
            
            try
            {
                // generate new trackingId and check if id already exists
                var isNewTrackingIdValid = false;
                var newTrackingId = "";

                while (!isNewTrackingIdValid)
                {
                    newTrackingId = GenerateTrackingId();
                    if (_parcelRepository.GetByTrackingId(newTrackingId)?.Id == null)
                    {
                        isNewTrackingIdValid = true;
                    }
                }

                parcel.TrackingId = newTrackingId;
                
                // get GPS coordinates for package sender/recipient
                var senderAddress = _mapper.Map<Elsa.SKS.Package.ServiceAgents.Entities.Address>(parcel.Sender);
                var recipientAddress = _mapper.Map<Elsa.SKS.Package.ServiceAgents.Entities.Address>(parcel.Recipient);
                var senderGps = _geocodingAgent.GeocodeAddress(senderAddress);
                var recipientGps = _geocodingAgent.GeocodeAddress(recipientAddress);

                // convert GPS coordinates to points
                var senderLocation = new Point(senderGps.Latitude, senderGps.Longitude);
                var recipientLocation = new Point(recipientGps.Latitude, recipientGps.Longitude);
                
                // get trucks that cover sender/recipient location
                var trucks = _hopRepository.GetAllTrucks();
                var senderTruck = trucks.First(t => t.GeoRegion.Contains(senderLocation));
                var recipientTruck = trucks.First(t => t.GeoRegion.Contains(recipientLocation));

                // get nearest warehouses
                var nearestWarehouseSender = senderTruck.PreviousHop.Warehouse;
                var nearestWarehouseReceiver = recipientTruck.PreviousHop.Warehouse;

                var parcelDal = _mapper.Map<Elsa.SKS.Package.DataAccess.Entities.Parcel>(parcel);
                var parcelEntity = _parcelRepository.Create(parcelDal);
                var result = _mapper.Map<Parcel>(parcelEntity);
                return result;
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Database error");
                throw new BusinessException("A database error has occurred.", ex);
            }
            
        }

        private string GenerateTrackingId()
        {
            const int stringLength = 9;
            var allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
                
            var randomId = new StringBuilder();
            var random = new Random();

            for (var i = 0; i < stringLength; i++)
            {
                var randomCharSelected = random.Next(0, (allowedChars.Length - 1));
                randomId.Append(allowedChars[randomCharSelected]);
            }

            return randomId.ToString();
        }
    }
}