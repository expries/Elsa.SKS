using Elsa.SKS.Package.BusinessLogic.Entities;

namespace Elsa.SKS.Package.BusinessLogic.Interfaces
{
    public interface IParcelRegistrationLogic
    {
        public Parcel TransitionParcel(Parcel parcel, string trackingId);
        
        public Parcel SubmitParcel(Parcel parcel);
    }
}