using FluentValidation;
using TestProject.ViewModels.Employees;

namespace TestProject.Validators.Employees;

public class AddEmployeeValidator : AbstractValidator<AddEmployeeDto>
{
    public AddEmployeeValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        RuleFor(x => x.FirstName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.LastName)
            .NotEmpty()
            .MaximumLength(50);

        RuleFor(x => x.MiddleName)
            .NotEmpty()
            .MaximumLength(50);
    }
}