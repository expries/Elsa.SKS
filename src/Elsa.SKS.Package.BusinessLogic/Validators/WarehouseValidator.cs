using System.Diagnostics.CodeAnalysis;
using Elsa.SKS.Package.BusinessLogic.Entities;
using FluentValidation;

namespace Elsa.SKS.Package.BusinessLogic.Validators
{
    [ExcludeFromCodeCoverage]
    public class WarehouseValidator : AbstractValidator<Warehouse>
    {
        public WarehouseValidator()
        {
            //RuleFor(w => w.Description).Matches("^[A-Za-z0-9\\-\\ ]*$");
            RuleFor(w => w.NextHops).NotNull();
            RuleFor(w => w.Code).NotEmpty();
        }
    }
}