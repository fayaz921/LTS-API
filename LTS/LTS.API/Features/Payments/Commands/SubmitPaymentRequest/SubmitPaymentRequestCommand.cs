using LTS.API.Common.Response;
using LTS.API.Domain.Enums;
using MediatR;

namespace LTS.API.Features.Payments.Commands.SubmitPaymentRequest
{
    public record SubmitPaymentRequestCommand(
       Guid OrganizationId,
       SubscriptionPlan RequestedPlan,
       string TransactionId,
       string SenderName,
       string SenderPhone,
       PaymentMethod PaymentMethod,
       decimal Amount,
       IFormFile Screenshot
   ) : IRequest<ApiResponse<string>>;
}
