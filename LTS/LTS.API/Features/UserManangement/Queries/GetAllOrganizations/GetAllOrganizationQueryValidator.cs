using FluentValidation;

namespace LTS.API.Features.UserManangement.Queries.GetAllOrganizations
{
    public class GetAllOrganizationQueryValidator:AbstractValidator<GetAllOrganizationsQuery>
    {
        public GetAllOrganizationQueryValidator()
        {
            RuleFor(x => x.PageNumber)
           .GreaterThan(0);

            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 100);
        }
    }
}
