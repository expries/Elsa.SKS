using Elsa.SKS.Backend.BusinessLogic.Entities;

namespace Elsa.SKS.Backend.BusinessLogic.Interfaces
{
    public interface IParcelRegistrationLogic
    {
        public Parcel TransitionParcel(Parcel parcel, string trackingId);

        public Parcel SubmitParcel(Parcel parcel);
    }
}