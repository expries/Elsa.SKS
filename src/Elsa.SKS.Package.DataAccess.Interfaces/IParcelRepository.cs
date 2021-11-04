using System;
using Elsa.SKS.Package.DataAccess.Entities;

namespace Elsa.SKS.Package.DataAccess.Interfaces
{
    public interface IParcelRepository
    {
        public Parcel Create(Parcel parcel);
        public Parcel Update(Parcel parcel);
        public bool Delete(Parcel parcel);
        //public bool ReportParcelHopArrival(string trackingId);
        public Parcel GetParcelByTrackingId(string trackingId);
        public bool DoesExist(string trackingId);
    }
}