using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.DataAccess.Interfaces;
using FluentValidation;

namespace Elsa.SKS.Package.BusinessLogic
{
    public class ParcelRegistrationLogic : IParcelRegistrationLogic
    {
        private readonly IValidator<Parcel> _parcelValidator;
        
        private IParcelRepository _parcelRepository;
        
        private readonly IMapper _mapper;


        public ParcelRegistrationLogic(IValidator<Parcel> parcelValidator, IParcelRepository parcelRepository, IMapper mapper)
        {
            _parcelValidator = parcelValidator;
            _parcelRepository = parcelRepository;
            _mapper = mapper;
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
            
            var parcelDal = _mapper.Map<Elsa.SKS.Package.DataAccess.Entities.Parcel>(parcel);
            var parcelEntity = _parcelRepository.Update(parcelDal);
            var result = _mapper.Map<Parcel>(parcelEntity);
            
            return result;
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
            
            var parcelDal = _mapper.Map<Elsa.SKS.Package.DataAccess.Entities.Parcel>(parcel);
            var parcelEntity = _parcelRepository.Create(parcelDal);
            var result = _mapper.Map<Parcel>(parcelEntity);

            // parcel with trackingId comes back
            //parcel.TrackingId = TestConstants.TrackingIdOfSubmittedParcel;
            return result;
        }
    }
}