using System.Diagnostics.CodeAnalysis;
using Elsa.SKS.Backend.BusinessLogic.Entities;
using FluentValidation;

namespace Elsa.SKS.Backend.BusinessLogic.Validators
{
    [ExcludeFromCodeCoverage]
    public class ParcelValidator : AbstractValidator<Parcel>
    {
        public ParcelValidator()
        {
            RuleFor(p => p.TrackingId).Matches("^[A-Z0-9]{9}$");
            RuleFor(p => p.Weight).GreaterThanOrEqualTo(0);
            RuleFor(p => p.Recipient).NotNull();
            RuleFor(p => p.Sender).NotNull();
            RuleFor(p => p.VisitedHops).NotNull();
            RuleFor(p => p.FutureHops).NotNull();
        }
    }
}