using Elsa.SKS.Package.BusinessLogic.Entities;
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
            throw new System.NotImplementedException();
        }

        public Parcel TrackParcel(string trackingId)
        {
            throw new System.NotImplementedException();
        }
    }
}