using Elsa.SKS.Package.BusinessLogic.Entities;
using FluentValidation;

namespace Elsa.SKS.Package.BusinessLogic.Validators
{
    public class AddressValidator : AbstractValidator<User>
    {
        public AddressValidator()
        {
            RuleFor(address => address.Country)
                .Must(country => country is "Austria" or "Österreich");

            RuleFor(address => address.PostalCode).Matches("(^A-[0-9]{4}$)");
            RuleFor(address => address.Street).Matches("(^([A-Za-zß]+ )*[0-9\\w\\/]*)");
            RuleFor(address => address.City).Matches("(^[A-Z]{1}[A-Za-z\\-\\ ]*$)");
        }
    }
}