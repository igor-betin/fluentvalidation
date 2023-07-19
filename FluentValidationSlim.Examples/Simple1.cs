using FluentValidationSlim.CommonValidationExtensions;
using FluentValidationSlim.Examples.Domain;
using FluentValidationSlim.Examples.Validators;
using FluentValidationSlim.Infra;

namespace FluentValidationSlim.Examples;

internal class Simple1
{
    private readonly PersonValidator _personValidator = new PersonValidator();
    
    public void Run()
    {
        var person1 = new Person { FirstName = "Igor", LastName = null, Age = 38 };

        var context = ValidationContextSlim.ForObject(person1);
        var result = new ValidationResultSlim();

        _personValidator.Validate(result, context);

        var message = string.Join(
            Environment.NewLine,
            result.Errors.Select(e => e.PropertyPath + ":" + e.Message));
        Console.WriteLine(message);
    }
}