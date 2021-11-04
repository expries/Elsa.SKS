using Elsa.SKS.Package.DataAccess.Entities;

namespace Elsa.SKS.Package.DataAccess.Interfaces
{
    public interface IParcelRepository
    {
        public Parcel Create(Parcel parcel);
        
        public Parcel Update(Parcel parcel);
        
        public bool Delete(Parcel parcel);

        public Parcel? GetByTrackingId(string trackingId);
    }
}