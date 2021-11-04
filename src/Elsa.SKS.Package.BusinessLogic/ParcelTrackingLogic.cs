using System;
using System.Linq;
using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
using Parcel = Elsa.SKS.Package.BusinessLogic.Entities.Parcel;
using DataAccessParcel = Elsa.SKS.Package.DataAccess.Entities.Parcel;

namespace Elsa.SKS.Package.BusinessLogic
{
    public class ParcelTrackingLogic : IParcelTrackingLogic
    {
        private readonly IParcelRepository _parcelRepository;
        
        private readonly IHopRepository _hopRepository;

        private readonly IMapper _mapper;

        public ParcelTrackingLogic(IParcelRepository parcelRepository, IHopRepository hopRepository, IMapper mapper)
        {
            _parcelRepository = parcelRepository;
            _hopRepository = hopRepository;
            _mapper = mapper;
        }

        public void ReportParcelDelivery(string trackingId)
        {
            try
            {
                var parcelEntity = _parcelRepository.GetByTrackingId(trackingId);

                // check that parcel exists
                if (parcelEntity is null)
                {
                    throw new ParcelNotFoundException($"Parcel with tracking id {trackingId} was not found");
                }

                var parcel = _mapper.Map<Parcel>(parcelEntity);

                // on delivery, mark all future hop arrivals with the current timestamp and add to visited hop
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
                    throw new ParcelNotFoundException($"Parcel with tracking id {trackingId} was not found");
                }

                var hopEntity = _hopRepository.GetByCode(code);

                // check that hop exists
                if (hopEntity is null)
                {
                    throw new HopNotFoundException($"Hop with code {code} was not found");
                }

                var parcel = _mapper.Map<Parcel>(parcelEntity);
                var hop = _mapper.Map<Hop>(hopEntity);
            
                // remove all future/visited hop arrivals with the given code
                parcel.FutureHops.RemoveAll(ha => ha.Hop.Code == code);
                parcel.VisitedHops.RemoveAll(ha => ha.Hop.Code == code);

                // add hop arrival to parcel's visited hops
                var hopArrival = new HopArrival { Hop = hop, DateTime = DateTime.Now };
                parcel.VisitedHops.Add(hopArrival);
            
                parcelEntity = _mapper.Map<DataAccessParcel>(parcel);
                _parcelRepository.Update(parcelEntity);
            }
            catch (DataAccessException ex)
            {
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
                    throw new ParcelNotFoundException($"Parcel with tracking id {trackingId} was not found");
                }

                var parcel = _mapper.Map<Parcel>(parcelEntity);
                return parcel;
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("A database error has occurred.", ex);
            }
        }
    }
}