using System;
using System.Text;
using System.Threading;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
using Elsa.SKS.Package.ServiceAgents.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;

namespace Elsa.SKS.Package.BusinessLogic
{
    public class ParcelRegistrationLogic : IParcelRegistrationLogic
    {
        private readonly IParcelRepository _parcelRepository;
        
        private readonly IValidator<Parcel> _parcelValidator;

        private readonly IMapper _mapper;
        
        private readonly ILogger<ParcelRegistrationLogic> _logger;

        private readonly IGeocodingAgent _geocodingAgent;

        public ParcelRegistrationLogic(IParcelRepository parcelRepository, IValidator<Parcel> parcelValidator, IMapper mapper, ILogger<ParcelRegistrationLogic> logger, IGeocodingAgent geocodingAgent)
        {
            _parcelRepository = parcelRepository;
            _parcelValidator = parcelValidator;
            _mapper = mapper;
            _logger = logger;
            _geocodingAgent = geocodingAgent;
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
            var senderGPS = _geocodingAgent.GeocodeAddress(senderAddress);
            var recipientAddress = _mapper.Map<Elsa.SKS.Package.ServiceAgents.Entities.Address>(parcel.Recipient);
            var recipientGPS = _geocodingAgent.GeocodeAddress(recipientAddress);
            
            var parcelDal = _mapper.Map<Elsa.SKS.Package.DataAccess.Entities.Parcel>(parcel);

            try
            {
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