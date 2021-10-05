using Elsa.SKS.Package.BusinessLogic.Entities;

namespace Elsa.SKS.Package.BusinessLogic.Interfaces
{
    public interface IParcelTracking
    {
        public Parcel ReportParcelDelivery(string trackingId);
            
        public void ReportParcelHop(string trackingId, string code);

        public Parcel TrackParcel(string trackingId);

    }
}