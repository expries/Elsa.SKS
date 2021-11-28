using System;
using System.Linq;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Entities.Enums;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
using Elsa.SKS.Package.ServiceAgents.Interfaces;
using Microsoft.Extensions.Logging;
using DataAccessParcel = Elsa.SKS.Package.DataAccess.Entities.Parcel;

namespace Elsa.SKS.Package.BusinessLogic
{
    public class ParcelTrackingLogic : IParcelTrackingLogic
    {
        private readonly IParcelRepository _parcelRepository;
        
        private readonly IHopRepository _hopRepository;
        
        private readonly ILogisticsPartnerAgent _logisticsPartner;

        private readonly IMapper _mapper;
        
        private readonly ILogger<ParcelTrackingLogic> _logger;

        public ParcelTrackingLogic(IParcelRepository parcelRepository, IHopRepository hopRepository, ILogisticsPartnerAgent logisticsPartner, IMapper mapper, ILogger<ParcelTrackingLogic> logger)
        {
            _parcelRepository = parcelRepository;
            _hopRepository = hopRepository;
            _logisticsPartner = logisticsPartner;
            _mapper = mapper;
            _logger = logger;
        }

        public void ReportParcelDelivery(string trackingId)
        {
            try
            {
                var parcelEntity = _parcelRepository.GetByTrackingId(trackingId);

                // check that parcel exists
                if (parcelEntity is null)
                {
                    _logger.LogInformation("Parcel with tracking id was not found");
                    throw new ParcelNotFoundException($"Parcel with tracking id {trackingId} was not found");
                }

                var parcel = _mapper.Map<Parcel>(parcelEntity);

                // update parcel state
                parcel.State = ParcelState.Delivered;

                // mark all future hops (if any) as reached
                parcel.FutureHops.ToList().ForEach(ha =>
                {
                    parcel.FutureHops.Remove(ha);
                    ha.DateTime = DateTime.Now;
                    parcel.VisitedHops.Add(ha);
                });

                parcelEntity = _mapper.Map<DataAccessParcel>(parcel);
                _parcelRepository.Update(parcelEntity);
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Database error");
                throw new BusinessException("A database error has occurred.", ex);
            }
        }

        public void ReportParcelHop(string trackingId, string code)
        {
            try
            {
                var parcelEntity = _parcelRepository.GetByTrackingId(trackingId);

                // check that parcel exists
                if (parcelEntity is null)
                {
                    _logger.LogInformation("Parcel with tracking id was not found");
                    throw new ParcelNotFoundException($"Parcel with tracking id {trackingId} was not found");
                }

                var hopEntity = _hopRepository.GetByCode(code);

                // check that hop exists
                if (hopEntity is null)
                {
                    _logger.LogInformation("Hop was not found");
                    throw new HopNotFoundException($"Hop with code {code} was not found");
                }

                var parcel = _mapper.Map<Parcel>(parcelEntity);
                var hop = _mapper.Map<Hop>(hopEntity);
            
                // remove hop with given code from future hops 
                parcel.FutureHops.RemoveAll(ha => ha.Hop.Code == code);

                // add hop arrival to parcel's visited hops
                var hopArrival = new HopArrival { Hop = hop, DateTime = DateTime.Now };
                parcel.VisitedHops.Add(hopArrival);

                // update parcel state
                switch (hop)
                {
                    case Warehouse:
                        parcel.State = ParcelState.InTransport;
                        break;
                    
                    case Truck:
                        parcel.State = ParcelState.InTruckDelivery;
                        break;
                    
                    case TransferWarehouse transferWarehouse:
                        _logisticsPartner.TransferParcel(transferWarehouse, parcel);
                        parcel.State = ParcelState.Transferred;
                        break;
                }

                parcelEntity = _mapper.Map<DataAccessParcel>(parcel);
                _parcelRepository.Update(parcelEntity);
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Database error");
                throw new BusinessException("A database error has occurred.", ex);
            }
        }

        public Parcel TrackParcel(string trackingId)
        {
            try
            {
                var parcelEntity = _parcelRepository.GetByTrackingId(trackingId);

                // check that parcel exists
                if (parcelEntity is null)
                {
                    _logger.LogInformation("Parcel with tracking id was not found");
                    throw new ParcelNotFoundException($"Parcel with tracking id {trackingId} was not found.");
                }

                var parcel = _mapper.Map<Parcel>(parcelEntity);
                return parcel;
            }
            catch (DataAccessException ex)
            {
                _logger.LogError(ex, "Database error");
                throw new BusinessException("A database error has occurred.", ex);
            }
        }
    }
}