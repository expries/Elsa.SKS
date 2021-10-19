using System;
using Elsa.SKS.Package.BusinessLogic.Entities;

namespace Elsa.SKS.Package.DataAccess.Interfaces
{
    public interface IParcelRepository
    {
        public Parcel Create(Parcel parcel);
        public bool Update(Parcel parcel);
        public bool Delete(Parcel parcel);
        public Parcel GetParcelByTrackingId(string trackingId);
    }
}