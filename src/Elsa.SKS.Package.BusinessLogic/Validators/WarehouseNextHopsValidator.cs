using System.Diagnostics.CodeAnalysis;
using Elsa.SKS.Package.Services.DTOs;
using FluentValidation;

namespace Elsa.SKS.Package.BusinessLogic.Validators
{
    [ExcludeFromCodeCoverage]
    public class WarehouseNextHopsValidator : AbstractValidator<WarehouseNextHops>
    {
        public WarehouseNextHopsValidator()
        {
            RuleFor(h => h.Hop).NotNull();
        }   
    }
}