using System.Diagnostics.CodeAnalysis;
using Elsa.SKS.Backend.Services.DTOs;
using FluentValidation;

namespace Elsa.SKS.Backend.BusinessLogic.Validators
{
    [ExcludeFromCodeCoverage]
    public class WarehouseNextHopsValidator : AbstractValidator<WarehouseNextHops>
    {
        public WarehouseNextHopsValidator()
        {
            RuleFor(h => h.NextHop).NotNull();
        }   
    }
}