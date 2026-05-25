using LTS.API.Domain.Enums;

namespace LTS.API.Domain.Constants
{
    public static class PlanConfig
    {
        public static readonly Dictionary<SubscriptionPlan, PlanDetails> Plans = new()
        {
            [SubscriptionPlan.Trial] = new(
                MaxUsers: 2,
                MaxPetitioners: 5,
                MaxCases: 10,
                Price: 0,
                DurationDays: 14
            ),
            [SubscriptionPlan.Starter] = new(
                MaxUsers: 3,
                MaxPetitioners: 20,
                MaxCases: 50,
                Price: 2500,
                DurationDays: 30
            ),
            [SubscriptionPlan.Professional] = new(
                MaxUsers: 10,
                MaxPetitioners: int.MaxValue,
                MaxCases: int.MaxValue,
                Price: 5000,
                DurationDays: 30
            ),
            [SubscriptionPlan.Enterprise] = new(
                MaxUsers: int.MaxValue,
                MaxPetitioners: int.MaxValue,
                MaxCases: int.MaxValue,
                Price: 10000,
                DurationDays: 30
            ),
        };

        public static PlanDetails GetPlan(SubscriptionPlan plan) => Plans[plan];
    }

    public record PlanDetails(
        int MaxUsers,
        int MaxPetitioners,
        int MaxCases,
        decimal Price,
        int DurationDays
    );

}
