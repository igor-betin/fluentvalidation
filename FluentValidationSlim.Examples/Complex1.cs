using FluentValidationSlim.CommonValidationExtensions;
using FluentValidationSlim.Examples.Domain;
using FluentValidationSlim.Examples.Validators;
using FluentValidationSlim.Infra;

namespace FluentValidationSlim.Examples;

internal class Complex1
{
    private readonly TouristGroupRequestValidator _touristGroupRequestValidator = new TouristGroupRequestValidator();

    public void Run()
    {
        var request = new TouristGroupRequest
        {
            Guide = null,
            Tourists = new List<Person>() {
                new Person { FirstName = "John", LastName = null },
                new Person { FirstName = null, LastName = "Johnson" },
                new Person { FirstName = "John", LastName = "Johnson" },
                new Person { FirstName = "Mary", LastName = "Jane" },
            }
        };
        
        var context = ValidationContextSlim.ForObject(request);
        var result = new ValidationResultSlim();

        _touristGroupRequestValidator.Validate(result, context);

        var message = string.Join(
            Environment.NewLine,
            result.Errors.Select(e => e.PropertyPath + ":" + e.Message));
        Console.WriteLine(message);
    }
}