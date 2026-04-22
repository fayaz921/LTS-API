using FluentValidation;

namespace LTS.API.Features.CaseFeature.Commands.CreateCase
{
    public class CreateCaseValidator : AbstractValidator<CreateCaseCommand>
    {
        public CreateCaseValidator()
        {
            // Title & Subject (Strings)
            RuleFor(x => x.Title)
                .NotEmpty().WithMessage("Title is required.")
                .MaximumLength(500).WithMessage("Title cannot exceed 500 characters.");

            RuleFor(x => x.Subject)
                .NotEmpty().WithMessage("Subject is required.")
                .MaximumLength(1000).WithMessage("Subject cannot exceed 1000 characters.");

            // Guids (Court & Department)
            RuleFor(x => x.CourtId)
                .NotEmpty().WithMessage("Please select a valid Court.");

            RuleFor(x => x.DepartmentId)
                .NotEmpty().WithMessage("Please select a valid Department.");

            // Date (Future dates se bachne ke liye)
            RuleFor(x => x.DateInstitution)
                .NotEmpty().WithMessage("Institution Date is required.");
                //.LessThanOrEqualTo(DateTime.Now).WithMessage("Institution Date cannot be in the future.");

            // Email List (Agar comma separated emails hain to simple regex check)
            RuleFor(x => x.EmailList)
                .MaximumLength(1000).WithMessage("Email list is too long.");

            // Detail (Optional ho sakta hai lekin length check zaroori hai)
            RuleFor(x => x.Detail)
                .NotEmpty().WithMessage("Details are required.");

            // OrganizationId (Safety check)
            RuleFor(x => x.OrganizationId)
                .NotEmpty().WithMessage("Organization context is missing.");
        }
    }
}
