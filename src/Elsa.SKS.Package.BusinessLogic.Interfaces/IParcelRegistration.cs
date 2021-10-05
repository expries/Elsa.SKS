using Elsa.SKS.Package.Services.DTOs;

namespace Elsa.SKS.Package.BusinessLogic.Interfaces
{
    public interface IParcelRegistration
    {
        public Parcel TransitionParcel(Parcel body, string trackingId);
        
        public Parcel SubmitParcel(Parcel body);
    }
}