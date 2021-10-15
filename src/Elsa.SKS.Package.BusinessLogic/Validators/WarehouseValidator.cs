using Elsa.SKS.Package.BusinessLogic.Entities;
using FluentValidation;

namespace Elsa.SKS.Package.BusinessLogic.Validators
{
    public class WarehouseValidator : AbstractValidator<Warehouse>
    {
        public WarehouseValidator()
        {
            RuleFor(w => w.Description).Matches("^[A-Za-z0-9\\- ]*$");
            RuleFor(w => w.NextHops).NotNull();
        }
    }
}