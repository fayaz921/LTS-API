using LTS.API.Domain.Entities;
using LTS.API.Domain.Enums;
using LTS.API.Features.UserManangement.DTOs;

namespace LTS.API.Features.UserManangement.Mappers
{
    public static class GetOrganizationMapping
    {
        public static OrganizationDto ToDto(this Organization org,Dictionary<Guid, int> petitionerCounts,Dictionary<Guid, int> caseCounts)
        {
            return new OrganizationDto
            {
                Id = org.Id,
                OrganizationName = org.OrganizationName,
                Slug = org.Slug,
                Plan = org.Plan.ToString(),
                IsActive = org.IsActive,
                IsBlocked = org.IsBlocked,
                MaxUsers = org.MaxUsers,
                MaxPetitioners = org.MaxPetitioners,
                MaxCases = org.MaxCases,
                CurrentUserCount = org.Users.Count,
                CurrentPetitionerCount = petitionerCounts.GetValueOrDefault(org.Id, 0),
                CurrentCaseCount = caseCounts.GetValueOrDefault(org.Id, 0),
                IsTrialActive = org.IsTrialActive,
                TrialStartDate = org.TrialStartDate,
                TrialEndDate = org.TrialEndDate,
                IsSubscriptionActive = org.IsSubscriptionActive,
                SubscriptionStartDate = org.SubscriptionStartDate,
                SubscriptionEndDate = org.SubscriptionEndDate,
                TotalPaymentRequests = org.PaymentRequests.Count,
                TotalAmountPaid = org.PaymentRequests
                                            .Where(p => p.Status == PaymentStatus.Approved)
                                            .Sum(p => p.Amount),
                LastPaymentStatus = org.PaymentRequests
                                            .OrderByDescending(p => p.SubmittedAt)
                                            .FirstOrDefault()?.Status.ToString(),
                LastPaymentDate = org.PaymentRequests
                                            .OrderByDescending(p => p.SubmittedAt)
                                            .FirstOrDefault()?.SubmittedAt,

                CreatedAt = org.CreatedAt,
                UpdatedAt = org.UpdatedAt
            };
        }
        }
}
