using FluentValidation;

namespace LTS.API.Features.UserManangement.Queries.GetTrialOrganizations
{
    public class GetTrailOrganizationValidator: AbstractValidator<GetTrialOrganizationsQuery>
    {
        public GetTrailOrganizationValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("PageNumber must be greater than 0");
            RuleFor(x => x.PageSize).GreaterThan(0).WithMessage("PageSize must be greater than 0");
            RuleFor(x => x.PageSize).LessThanOrEqualTo(100).WithMessage("PageSize cannot exceed 100");
        }
    }
}
