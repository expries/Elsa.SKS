using Elsa.SKS.Backend.BusinessLogic.Entities;

namespace Elsa.SKS.Backend.BusinessLogic.Interfaces
{
    public interface IParcelTrackingLogic
    {
        public void ReportParcelDelivery(string trackingId);

        public void ReportParcelHop(string trackingId, string code);

        public Parcel TrackParcel(string trackingId);

    }
}