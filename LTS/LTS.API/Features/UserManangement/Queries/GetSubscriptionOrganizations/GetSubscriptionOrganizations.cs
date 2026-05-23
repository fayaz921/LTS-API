using FluentValidation;

namespace LTS.API.Features.UserManangement.Queries.GetSubscriptionOrganizations
{
    public class GetSubscriptionOrganizations:AbstractValidator<GetSubscriptionOrganizationsQuery>
    {
        public GetSubscriptionOrganizations()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0).LessThanOrEqualTo(100);
        }
    }
}
