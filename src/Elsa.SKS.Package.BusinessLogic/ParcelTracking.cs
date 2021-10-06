using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;

namespace Elsa.SKS.Package.BusinessLogic
{
    public class ParcelTracking : IParcelTracking
    {
        public Parcel ReportParcelDelivery(string trackingId)
        {
            throw new System.NotImplementedException();
        }

        public void ReportParcelHop(string trackingId, string code)
        {
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
        }

        public Parcel TrackParcel(string trackingId)
        {
            switch (trackingId)
            {
                case TestConstants.TrackingIdOfNonExistentParcel:
                    throw new ParcelNotFoundException($"Parcel with tracking id {trackingId} was not found");
                
                case TestConstants.TrackingIdOfParcelThatCanNotBeTracked:
                    throw new TrackingException($"Parcel with tracking id {trackingId} can not be tracked");
                
                default:
                    return new Parcel { TrackingId = trackingId };
            }
        }
    }
}