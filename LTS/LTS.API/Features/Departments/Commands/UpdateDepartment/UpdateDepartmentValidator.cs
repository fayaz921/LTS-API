using FluentValidation;

namespace LTS.API.Features.Departments.Commands.UpdateDepartment;
public class UpdateDepartmentValidator : AbstractValidator<UpdateDepartmentCommand>
{
    public UpdateDepartmentValidator()
    {
        RuleFor(x => x.DepartmentName)
            .NotEmpty().WithMessage("Department name is required")
            .MaximumLength(100).WithMessage("Max 100 characters allowed");

        RuleFor(x => x.AddressContact)
            .MaximumLength(500);
    }
}