using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.DataAccess.Interfaces;

namespace Elsa.SKS.Package.BusinessLogic
{
    public class ParcelTracking : IParcelTracking
    {
        private IParcelRepository _parcelRepository;

        private readonly IMapper _mapper;

        public ParcelTracking(IParcelRepository parcelRepository, IMapper mapper)
        {
            _parcelRepository = parcelRepository;
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

            /*
            if (trackingId == TestConstants.TrackingIdOfNonExistentParcel)
            {
                throw new ParcelNotFoundException($"Parcel with tracking id {trackingId} was not found");
            }

            if (trackingId == TestConstants.TrackingIdOfParcelThatCanNotBeReported)
            {
                throw new ReportParcelHopException("Parcel delivery cannot be reported for parcel with " +
                                                   $"tracking id {trackingId}");
            }
            */
            
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

            // TODO: IHopRepository?
            if (code == TestConstants.NonExistentHopCode)
            {
                throw new HopNotFoundException($"Hop with code {code} was not found");
            }

            if (!_parcelRepository.ReportParcelHopArrival(trackingId))
            {
                throw new ReportParcelHopException($"Hop with code {code} can not be reported for parcel " +
                                                   $"with tracking id {trackingId}");
            }
            
            /*
            if (trackingId == TestConstants.TrackingIdOfNonExistentParcel)
            {
                throw new ParcelNotFoundException($"Parcel with tracking id {trackingId} was not found");
            }

            if (code == TestConstants.NonExistentHopCode)
            {
                throw new HopNotFoundException($"Hop with code {code} was not found");
            }

            if (trackingId == TestConstants.TrackingIdOfParcelThatCanNotBeReported)
            {
                throw new ReportParcelHopException($"Hop with code {code} can not be reported for parcel " +
                                                   $"with tracking id {trackingId}");
            }
            */
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

            /*
            switch (trackingId)
            {
                case TestConstants.TrackingIdOfNonExistentParcel:
                    throw new ParcelNotFoundException($"Parcel with tracking id {trackingId} was not found");
                
                case TestConstants.TrackingIdOfParcelThatCanNotBeTracked:
                    throw new TrackingException($"Parcel with tracking id {trackingId} can not be tracked");
                
                default:
                    return new Parcel { TrackingId = trackingId };
            }
            */
        }
    }
}