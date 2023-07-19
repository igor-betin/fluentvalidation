using FluentValidationSlim.CommonValidationExtensions;
using FluentValidationSlim.Examples.Domain;
using FluentValidationSlim.Infra;

namespace FluentValidationSlim.Examples.Validators
{
    internal class PersonValidator
    {
        public bool Validate(ValidationResultSlim result, ValidationContextSlim<Person> context)
        {
            var ok = true;

            ok &= context.NotEmptyWithMessage(result, c => c.FirstName, ValidationMessages.FieldCanNotBeEmpty);
            ok &= context.NotEmptyWithMessage(result, c => c.LastName, ValidationMessages.FieldCanNotBeEmpty);

            return ok;
        }
    }
}
