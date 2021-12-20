using System;
using System.Linq;
using AutoMapper;
using Elsa.SKS.Backend.BusinessLogic.Entities;
using Elsa.SKS.Backend.BusinessLogic.Entities.Enums;
using Elsa.SKS.Backend.BusinessLogic.Entities.Events;
using Elsa.SKS.Backend.BusinessLogic.Exceptions;
using Elsa.SKS.Backend.BusinessLogic.Interfaces;
using Elsa.SKS.Backend.DataAccess.Interfaces;
using Elsa.SKS.Backend.DataAccess.Sql.Exceptions;
using Elsa.SKS.Backend.ServiceAgents.Exceptions;
using Elsa.SKS.Backend.ServiceAgents.Interfaces;
using Microsoft.Extensions.Logging;
using DataAccessParcel = Elsa.SKS.Backend.DataAccess.Entities.Parcel;

namespace Elsa.SKS.Backend.BusinessLogic
{
    public class ParcelTrackingLogic : IParcelTrackingLogic
    {
        private readonly IParcelRepository _parcelRepository;
        
        private readonly IHopRepository _hopRepository;
        
        private readonly ILogisticsPartnerAgent _logisticsPartner;

        private readonly IMapper _mapper;
        
        private readonly ILogger<ParcelTrackingLogic> _logger;

        private readonly IWebhookLogic _webhookLogic;
        
        public event EventHandler<ParcelEventArgs> ParcelDelivered;
        
        public event EventHandler<ParcelEventArgs> ParcelStatusChanged;


        public ParcelTrackingLogic(IParcelRepository parcelRepository, IHopRepository hopRepository, ILogisticsPartnerAgent logisticsPartner, IMapper mapper, ILogger<ParcelTrackingLogic> logger, IWebhookLogic webhookLogic)
        {
            _parcelRepository = parcelRepository;
            _hopRepository = hopRepository;
            _logisticsPartner = logisticsPartner;
            _mapper = mapper;
            _logger = logger;
            _webhookLogic = webhookLogic;

            ParcelDelivered += webhookLogic.OnParcelDelivered;
            ParcelStatusChanged += webhookLogic.OnParcelStatusChanged;
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
                
                // fire events
                _logger.LogInformation("ParcelStatusChanged event is fired");
                OnParcelStatusChanged(parcel);
                _logger.LogInformation("ParcelDelivered event is fired");
                OnParcelDelivered(parcel);

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
                if (parcel.VisitedHops.All(_ => _.Hop.Id != hop.Id))
                {
                    var hopArrival = new HopArrival { Hop = hop, DateTime = DateTime.Now };
                    parcel.VisitedHops.Add(hopArrival);
                }

                // update parcel state
                switch (hop)
                {
                    case Warehouse:
                        parcel.State = ParcelState.InTransport;
                        _logger.LogInformation("ParcelStatusChanged event is fired");
                        OnParcelStatusChanged(parcel);
                        break;
                    
                    case Truck:
                        parcel.State = ParcelState.InTruckDelivery;
                        _logger.LogInformation("ParcelStatusChanged event is fired");
                        OnParcelStatusChanged(parcel);
                        break;
                    
                    case TransferWarehouse transferWarehouse:
                        _logisticsPartner.TransferParcel(transferWarehouse, parcel);
                        parcel.State = ParcelState.Transferred;
                        _logger.LogInformation("ParcelStatusChanged event is fired");
                        OnParcelStatusChanged(parcel);
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
            catch (ServiceAgentException ex)
            {
                _logger.LogError(ex, "");
                throw new BusinessException(ex.Message, ex);
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
        
        protected virtual void OnParcelDelivered(Parcel parcel)
        {
            ParcelDelivered?.Invoke(this, new ParcelEventArgs() { Parcel = parcel });
        }
        
        protected virtual void OnParcelStatusChanged(Parcel parcel)
        {
            ParcelStatusChanged?.Invoke(this, new ParcelEventArgs() { Parcel = parcel });
        }
    }
}