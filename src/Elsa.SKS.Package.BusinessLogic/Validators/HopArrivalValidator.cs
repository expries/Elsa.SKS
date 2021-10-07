
using Elsa.SKS.Package.BusinessLogic.Entities;
using FluentValidation;

namespace Elsa.SKS.Package.BusinessLogic.Validators
{
    public class HopArrivalValidator : AbstractValidator<HopArrival>
    {
        public HopArrivalValidator()
        {
            RuleFor(a => a.Hop).NotNull();
        }
    }
}