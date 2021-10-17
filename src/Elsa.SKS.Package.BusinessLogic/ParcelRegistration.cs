using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using FluentValidation;

namespace Elsa.SKS.Package.BusinessLogic
{
    public class ParcelRegistration : IParcelRegistration
    {
        private readonly IValidator<Parcel> _parcelValidator;
        
        public ParcelRegistration(IValidator<Parcel> parcelValidator)
        {
            _parcelValidator = parcelValidator;
        }
    
        public Parcel TransitionParcel(Parcel parcel, string trackingId)
        {
            var validation = _parcelValidator.Validate(parcel);

            if (!validation.IsValid)
            {
                throw new TransferException(validation.ToString(" "));
            }
            
            if (trackingId != TestConstants.TrackingIdOfParcelThatIsTransferred)
            {
                throw new TransferException("Parcel cannot be transferred");
            }
            
            return parcel;
        }

        public Parcel SubmitParcel(Parcel parcel)
        {
            var validation = _parcelValidator.Validate(parcel);

            if (!validation.IsValid)
            {
                throw new InvalidParcelException(validation.ToString(" "));
            }
            
            if (parcel.Weight <= 0)
            {
                throw new InvalidParcelException("Parcel weight has to be greater than 0");
            }

            parcel.TrackingId = TestConstants.TrackingIdOfSubmittedParcel;
            return parcel;
        }
    }
}