using AutoMapper;
using Elsa.SKS.Package.BusinessLogic.Entities;
using Elsa.SKS.Package.BusinessLogic.Exceptions;
using Elsa.SKS.Package.BusinessLogic.Interfaces;
using Elsa.SKS.Package.DataAccess.Interfaces;
using Elsa.SKS.Package.DataAccess.Sql.Exceptions;
using FluentValidation;

namespace Elsa.SKS.Package.BusinessLogic
{
    public class ParcelRegistrationLogic : IParcelRegistrationLogic
    {
        private readonly IParcelRepository _parcelRepository;
        
        private readonly IValidator<Parcel> _parcelValidator;

        private readonly IMapper _mapper;
        
        public ParcelRegistrationLogic(IParcelRepository parcelRepository, IValidator<Parcel> parcelValidator, IMapper mapper)
        {
            _parcelRepository = parcelRepository;
            _parcelValidator = parcelValidator;
            _mapper = mapper;
        }
    
        public Parcel TransitionParcel(Parcel parcel, string trackingId)
        {
            var validation = _parcelValidator.Validate(parcel);

            if (!validation.IsValid)
            {
                throw new TransferException(validation.ToString(" "));
            }
            
            try
            {
                var existingParcel = _parcelRepository.GetByTrackingId(trackingId);

                if (existingParcel is not null)
                {
                    throw new TransferException("A parcel with this tracking Id is already being tracked.");
                }
            
                var parcelDal = _mapper.Map<Elsa.SKS.Package.DataAccess.Entities.Parcel>(parcel);
                var parcelEntity = _parcelRepository.Create(parcelDal);
                var result = _mapper.Map<Parcel>(parcelEntity);
                return result;
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("A database error has occurred.", ex);
            }
        }

        public Parcel SubmitParcel(Parcel parcel)
        {
            var validation = _parcelValidator.Validate(parcel);

            if (!validation.IsValid)
            {
                throw new InvalidParcelException(validation.ToString(" "));
            }
            
            var parcelDal = _mapper.Map<Elsa.SKS.Package.DataAccess.Entities.Parcel>(parcel);

            try
            {
                var parcelEntity = _parcelRepository.Create(parcelDal);
                var result = _mapper.Map<Parcel>(parcelEntity);
                return result;
            }
            catch (DataAccessException ex)
            {
                throw new BusinessException("A database error has occurred.", ex);
            }
        }
    }
}