using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using AutoMapper;
using Elsa.SKS.Backend.BusinessLogic.Entities;
using Elsa.SKS.Backend.BusinessLogic.Entities.Enums;
using Elsa.SKS.Backend.BusinessLogic.Exceptions;
using Elsa.SKS.Backend.BusinessLogic.Interfaces;
using Elsa.SKS.Backend.DataAccess.Interfaces;
using Elsa.SKS.Backend.DataAccess.Sql.Exceptions;
using Elsa.SKS.Backend.ServiceAgents.Exceptions;
using Elsa.SKS.Backend.ServiceAgents.Interfaces;
using FluentValidation;
using Microsoft.Extensions.Logging;
using NetTopologySuite.Geometries;

namespace Elsa.SKS.Backend.BusinessLogic
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

                parcel.TrackingId = trackingId;
                parcel.FutureHops = GetFutureHops(parcel.Sender, parcel.Recipient);
                parcel.State = ParcelState.Pickup;

                var parcelDal = _mapper.Map<Elsa.SKS.Backend.DataAccess.Entities.Parcel>(parcel);
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
                var newTrackingId = string.Empty;

                while (!isNewTrackingIdValid)
                {
                    newTrackingId = GenerateTrackingId();
                    if (_parcelRepository.GetByTrackingId(newTrackingId)?.Id == null)
                    {
                        isNewTrackingIdValid = true;
                    }
                }

                parcel.TrackingId = newTrackingId;
                parcel.FutureHops = GetFutureHops(parcel.Sender, parcel.Recipient);
                parcel.State = ParcelState.Pickup;

                var parcelDal = _mapper.Map<Elsa.SKS.Backend.DataAccess.Entities.Parcel>(parcel);
                var parcelEntity = _parcelRepository.Create(parcelDal);
                var result = _mapper.Map<Parcel>(parcelEntity);
                return result;
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Database error");
                throw new BusinessException("A database error has occurred.", ex);
            }
            catch (ServiceAgentException ex)
            {
                _logger.LogError(ex, "Service agent error");
                throw new BusinessException(ex.Message, ex);
            }
        }

        private string GenerateTrackingId()
        {
            const int stringLength = 9;
            char[] allowedChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789".ToCharArray();
                
            var randomId = new StringBuilder();
            var random = new Random();

            for (int i = 0; i < stringLength; i++)
            {
                int randomCharSelected = random.Next(0, (allowedChars.Length - 1));
                randomId.Append(allowedChars[randomCharSelected]);
            }

            return randomId.ToString();
        }

        private List<HopArrival> GetFutureHops(User parcelSender, User parcelRecipient)
        {
            // get GPS coordinates for package sender/recipient
            var senderAddress = _mapper.Map<Elsa.SKS.Backend.ServiceAgents.Entities.Address>(parcelSender);
            var recipientAddress = _mapper.Map<Elsa.SKS.Backend.ServiceAgents.Entities.Address>(parcelRecipient);
            var senderGps = _geocodingAgent.GeocodeAddress(senderAddress);
            var recipientGps = _geocodingAgent.GeocodeAddress(recipientAddress);

            // convert GPS coordinates to points
            var senderLocation = new Point(senderGps.Longitude, senderGps.Latitude);
            var recipientLocation = new Point(recipientGps.Longitude, recipientGps.Latitude);

            // get trucks that cover sender/recipient location
            var trucks = _hopRepository.GetAllTrucks();
            DataAccess.Entities.Hop senderHop = trucks.FirstOrDefault(t => t.GeoRegion.Contains(senderLocation));
            DataAccess.Entities.Hop recipientHop = trucks.FirstOrDefault(t => t.GeoRegion.Contains(recipientLocation));

            // if no truck covers location, check if transfer warehouse covers it
            if (senderHop is null || recipientHop is null)
            {
                var transferWarehouses = _hopRepository.GetAllTransferWarehouses();
                senderHop ??= transferWarehouses.FirstOrDefault(x => x.GeoRegion.Contains(senderLocation));
                recipientHop ??= transferWarehouses.FirstOrDefault(x => x.GeoRegion.Contains(recipientLocation));
            }

            if (senderHop is null)
            {
                _logger.LogWarning("Could not find truck/logistics partner for sender address.");
                throw new BusinessException("Could not find truck/logistics partner for sender address.");
            }
            
            if (recipientHop is null)
            {
                _logger.LogWarning("Could not find truck/logistics partner for recipient address.");
                throw new BusinessException("Could not find truck/logistics partner for sender address.");
            }

            var hopRoute = GetHopRoute(senderHop, recipientHop);
            return hopRoute.Select(hop => new HopArrival { Hop = hop }).ToList();
        }
        
        private List<Hop> GetHopRoute(DataAccess.Entities.Hop senderHop, DataAccess.Entities.Hop receiverHop)
        {
            // if sender and receiver truck is the same truck
            if (senderHop.Code == receiverHop.Code)
            {
                return new List<Hop>();
            }
            
            var routeSender = new List<Hop>();
            var routeReceiver = new List<Hop>();
            var currHopSender = _hopRepository.GetByCode(senderHop.Code); // start hop
            var currHopReceiver = _hopRepository.GetByCode(receiverHop.Code); // end hop
            
            routeSender.Add(_mapper.Map<Hop>(currHopSender));
            routeReceiver.Add(_mapper.Map<Hop>(currHopReceiver));
            
            while (currHopSender.Code != currHopReceiver.Code)
            {
                currHopSender = _hopRepository.GetByCode(currHopSender.ParentHop.Warehouse.Code);
                currHopReceiver = _hopRepository.GetByCode(currHopReceiver.ParentHop.Warehouse.Code);
                
                routeSender.Add(_mapper.Map<Hop>(currHopSender));
                routeReceiver.Add(_mapper.Map<Hop>(currHopReceiver));
            }

            var routeCombined = new List<Hop>();
            routeCombined.AddRange(routeSender);
            
            for (var i = routeReceiver.Count - 2; i == 0; i--)
            {
                routeCombined.Add(routeReceiver[i]);
            }

            return routeCombined;
        }
    }
}