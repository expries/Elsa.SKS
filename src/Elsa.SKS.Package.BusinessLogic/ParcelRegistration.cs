using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;

namespace Elsa.SKS.Package.BusinessLogic
{
    public class ParcelRegistration : IParcelRegistration
    {
        public Parcel TransitionParcel(Parcel parcel, string trackingId)
        {
            if (trackingId != TestConstants.TrackingIdOfParcelThatIsTransferred)
            {
                throw new TransferException("Parcel cannot be transferred");
            }
            
            return parcel;
        }

        public Parcel SubmitParcel(Parcel parcel)
        {
            if (parcel.Weight <= 0)
            {
                throw new InvalidParcelWeightException("Parcel weight has to be greater than 0");
            }

            parcel.TrackingId = TestConstants.TrackingIdOfSubmittedParcel;
            return parcel;
        }
    }
}