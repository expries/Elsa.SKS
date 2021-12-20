using Elsa.SKS.Backend.DataAccess.Entities;

namespace Elsa.SKS.Backend.DataAccess.Interfaces
{
    public interface IParcelRepository
    {
        public Parcel Create(Parcel parcel);

        public Parcel Update(Parcel parcel);

        public bool Delete(Parcel parcel);

        public Parcel? GetByTrackingId(string trackingId);
    }
}