using Elsa.SKS.Package.BusinessLogic.Entities;
using FluentValidation;

namespace Elsa.SKS.Package.BusinessLogic.Validators
{
    public class HopArrivalValidator : AbstractValidator<HopArrival>
    {
        public HopArrivalValidator()
        {
            RuleFor(hopArrival => hopArrival.Hop.Code).Matches("(^[A-Z]{4}\\d{1,4}$");
            RuleFor(a => a.Hop).NotNull();
        }
    }
}