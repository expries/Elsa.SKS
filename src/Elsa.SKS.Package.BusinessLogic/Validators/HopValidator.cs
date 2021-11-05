using System.Diagnostics.CodeAnalysis;
using Elsa.SKS.Package.BusinessLogic.Entities;
using FluentValidation;

namespace Elsa.SKS.Package.BusinessLogic.Validators
{
    [ExcludeFromCodeCoverage]
    public class HopValidator : AbstractValidator<Hop>
    {
        public HopValidator()
        {
            RuleFor(h => h.LocationCoordinates).NotNull();
        }
    }
}