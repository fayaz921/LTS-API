using LTS.API.Common.Response;
using MediatR;

namespace LTS.API.Features.Payments.Commands.ApprovePaymentRequest
{
    public record ApprovePaymentRequestCommand(
        Guid PaymentRequestId,
        string ReviewedBy
    ) : IRequest<ApiResponse<string>>;
}
