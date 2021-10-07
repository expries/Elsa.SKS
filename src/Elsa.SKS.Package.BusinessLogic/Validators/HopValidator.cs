using Elsa.SKS.Package.BusinessLogic.Entities;
using FluentValidation;

namespace Elsa.SKS.Package.BusinessLogic.Validators
{
    public class HopValidator : AbstractValidator<Hop>
    {
        public HopValidator()
        {
            RuleFor(h => h.LocationCoordinates).NotNull();
        }
    }
}