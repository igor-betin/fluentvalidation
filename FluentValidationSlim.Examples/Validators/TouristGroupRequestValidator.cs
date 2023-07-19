using FluentValidationSlim.Examples.Domain;
using FluentValidationSlim.Infra;
using FluentValidationSlim.CommonValidationExtensions;
using FluentValidationSlim.Examples.ValidationExtensions;

namespace FluentValidationSlim.Examples.Validators
{
    internal class TouristGroupRequestValidator
    {
        private readonly PersonValidator _personValidator = new PersonValidator();

        public bool Validate(ValidationResultSlim result, ValidationContextSlim<TouristGroupRequest> context)
        {
            var ok = true;

            var guide = context.SpawnForChild(c => c.Guide);
            if (guide.NotEmptyWithMessage(result, ValidationMessages.FieldCanNotBeEmpty))
            {
                ok &= _personValidator.Validate(result, guide);
            }
            else
                ok = false;

            var tourists = context.SpawnForChild(c => c.Tourists);
            tourists.MaxCount(result, 1, 3);

            foreach (var tourist in tourists.SpawnForList<Person>())
            {
                ok &= _personValidator.Validate(result, tourist);
            }

            return ok;
        }
    }
}
