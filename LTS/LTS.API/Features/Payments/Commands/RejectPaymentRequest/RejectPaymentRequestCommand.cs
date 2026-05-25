using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.Payments.Commands.RejectPaymentRequest
{
    public record RejectPaymentRequestCommand(
        Guid PaymentRequestId,
        string RejectionReason,
        string ReviewedBy
    ) : IRequest<ApiResponse<string>>;
}
