using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.DataAccess.Interfaces;

namespace Elsa.SKS.Package.BusinessLogic
{
    public class ParcelTrackingLogic : IParcelTrackingLogic
    {
        private IParcelRepository _parcelRepository;
        
        private IHopRepository _hopRepository;

        private readonly IMapper _mapper;

        public ParcelTrackingLogic(IParcelRepository parcelRepository, IHopRepository hopRepository, IMapper mapper)
        {
            _parcelRepository = parcelRepository;
            _hopRepository = hopRepository;
            _mapper = mapper;
        }

        public Parcel ReportParcelDelivery(string trackingId)
        {
            
            if (!_parcelRepository.DoesExist(trackingId))
            {
                throw new ParcelNotFoundException($"Parcel with tracking id {trackingId} was not found");
            }

            if (!_parcelRepository.ReportParcelHopArrival(trackingId))
            {
                throw new ReportParcelHopException("Parcel delivery cannot be reported for parcel with " +
                                                   $"tracking id {trackingId}");
            }
            
            var parcelEntity = _parcelRepository.GetParcelByTrackingId(trackingId);
            var result = _mapper.Map<Parcel>(parcelEntity);

            return result;
        }

        public void ReportParcelHop(string trackingId, string code)
        {
            
            if (!_parcelRepository.DoesExist(trackingId))
            {
                throw new ParcelNotFoundException($"Parcel with tracking id {trackingId} was not found");
            }

            if (!_hopRepository.IsValidHopCode(code))
            {
                throw new HopNotFoundException($"Hop with code {code} was not found");

            }
            
            //TODO: logic hier implementieren
            if (!_parcelRepository.ReportParcelHopArrival(trackingId))
            {
                throw new ReportParcelHopException($"Hop with code {code} can not be reported for parcel " +
                                                   $"with tracking id {trackingId}");
            }
        }

        public Parcel TrackParcel(string trackingId)
        {
            if (!_parcelRepository.DoesExist(trackingId))
            {
                throw new ParcelNotFoundException($"Parcel with tracking id {trackingId} was not found");
            }

            var parcelEntity = _parcelRepository.GetParcelByTrackingId(trackingId);
            var result = _mapper.Map<Parcel>(parcelEntity);

            if (result == null)
            {
                throw new TrackingException($"Parcel with tracking id {trackingId} can not be tracked");
            }
            
            return result;
            
        }
    }
}